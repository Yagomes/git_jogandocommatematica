<?php
// Conexão com o banco de dados
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

// Recebe o ID da turma enviado pela Unity
$turma_id = isset($_POST['turma_id']) ? $_POST['turma_id'] : 0;

$response = new stdClass(); // Cria um objeto para a resposta

// Consulta para buscar os tópicos associados à turma
$sql = "SELECT t.topico_nome, t.topico_num_min, t.topico_num_max
        FROM topico t
        INNER JOIN turma_topico tt ON t.topico_id = tt.topico_id
        WHERE tt.turma_id = ?";
$stmt = $conn->prepare($sql);

if ($stmt) {
    $stmt->bind_param("i", $turma_id);
    $stmt->execute();
    $result = $stmt->get_result();

    $topicos = []; // Cria um array para armazenar os tópicos

    // Itera sobre os resultados e adiciona ao array
    while ($row = $result->fetch_assoc()) {
        $topico = new stdClass();
        $topico->topico_nome = $row['topico_nome']; // Nome do tópico
        $topico->topico_num_min = $row['topico_num_min']; // Valor mínimo
        $topico->topico_num_max = $row['topico_num_max']; // Valor máximo
        $topicos[] = $topico; // Adiciona o tópico ao array
    }

    $response->topicos = $topicos; // Adiciona os tópicos na resposta
    $response->status = count($topicos) > 0 ? "success" : "fail"; // Define o status

    $stmt->close();
} else {
    $response->status = "fail";
    $response->erro = "Erro na preparação da consulta SQL.";
}

// Debug para verificar os dados enviados e consulta realizada
$response->debug = [
    "turma_id" => $turma_id,
    "consulta" => $sql
];

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>

