<?php
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

$aluno_matricula = isset($_POST['aluno_matricula']) ? $_POST['aluno_matricula'] : '';
$aluno_senha = isset($_POST['aluno_senha']) ? $_POST['aluno_senha'] : '';

$sql = "SELECT aluno_id, turma_id, aluno_genero FROM aluno WHERE aluno_matricula = ? AND aluno_senha = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $aluno_matricula, $aluno_senha);
$stmt->execute();
$result = $stmt->get_result();

$response = new stdClass();

if ($row = $result->fetch_assoc()) {
    $response->aluno_id = $row['aluno_id'];
    $response->turma_id = $row['turma_id'];
    $response->aluno_genero = $row['aluno_genero'];
    $response->status = "success";
} else {
    $response->erro = "Login inválido";
    $response->status = "fail";
}

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>



