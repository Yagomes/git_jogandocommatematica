<?php
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

$aluno_id = isset($_POST['aluno_id']) ? $_POST['aluno_id'] : '';

$sql = "SELECT * FROM estatistica WHERE aluno_id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $aluno_id);
$stmt->execute();
$result = $stmt->get_result();

$response = new stdClass();

if ($row = $result->fetch_assoc()) {
    $response->estatistica_total_jogado = $row['estatistica_total_jogado'];
    $response->estatistica_acertos = $row['estatistica_acertos'];
    $response->estatistica_erros = $row['estatistica_erros'];
    $response->estatistica_inimigos_derrotados = $row['estatistica_inimigos_derrotados'];
    $response->estatistica_moedas_acumuladas = $row['estatistica_moedas_acumuladas'];
    $response->estatistica_niveis_desbloqueados = $row['estatistica_niveis_desbloqueados'];
    $response->status = "success";
} else {
    $response->erro = "Estatisticas nao encontradas";
    $response->status = "fail";
}

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>
