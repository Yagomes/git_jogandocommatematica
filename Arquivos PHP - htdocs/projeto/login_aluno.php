<?php
// Configuração da conexão com o banco de dados
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

// Recebendo os dados enviados pela Unity
$matricula = isset($_POST['matricula']) ? $_POST['matricula'] : '';
$senha = isset($_POST['senha']) ? $_POST['senha'] : '';

// Validando as credenciais
$sql = "SELECT id_Aluno AS id_Aluno, id_turma FROM aluno WHERE matricula = ? AND senha = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $matricula, $senha);
$stmt->execute();
$result = $stmt->get_result();

// Criando a resposta como objeto JSON
$response = new stdClass();

if ($row = $result->fetch_assoc()) {
    $response->id_Aluno = $row['id_Aluno'];
    $response->id_turma = $row['id_turma'];
    $response->status = "success";
} else {
    $response->erro = "Login inválido";
    $response->status = "fail";
}

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>


