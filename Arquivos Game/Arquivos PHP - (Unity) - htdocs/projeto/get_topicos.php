<?php
// Conexão com o banco de dados
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

// Recebe o ID da turma enviado pela Unity
$id_turma = isset($_POST['id_turma']) ? $_POST['id_turma'] : 0;

$response = new stdClass(); // Cria um objeto para a resposta

// Consulta para buscar os tópicos associados à turma
$sql = "SELECT t.Nome_topico, t.Num_Min_Topico, t.Num_Max_Topico
        FROM topico t
        INNER JOIN turma_topico tt ON t.id_topico = tt.id_topico
        WHERE tt.id_Turma = ?";
$stmt = $conn->prepare($sql);

if ($stmt) {
    $stmt->bind_param("i", $id_turma);
    $stmt->execute();
    $result = $stmt->get_result();

    $topicos = []; // Cria um array para armazenar os tópicos

    // Itera sobre os resultados e adiciona ao array
    while ($row = $result->fetch_assoc()) {
        $topico = new stdClass();
        $topico->Nome_topico = $row['Nome_topico']; // Nome do tópico
        $topico->Num_Min_topico = $row['Num_Min_Topico']; // Valor mínimo
        $topico->Num_Max_topico = $row['Num_Max_Topico']; // Valor máximo
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
    "id_turma" => $id_turma,
    "consulta" => $sql
];

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>

