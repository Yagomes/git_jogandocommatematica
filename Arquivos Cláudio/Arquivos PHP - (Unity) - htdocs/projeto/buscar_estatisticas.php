<?php
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

$aluno_id = isset($_POST['id_Aluno']) ? $_POST['id_Aluno'] : '';

$sql = "SELECT * FROM estatisticas WHERE id_jogador = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $aluno_id);
$stmt->execute();
$result = $stmt->get_result();

$response = new stdClass();

if ($row = $result->fetch_assoc()) {
    $response->total_jogado = $row['total_jogado'];
    $response->acertos = $row['acertos'];
    $response->erros = $row['erros'];
    $response->inimigos_derrotados = $row['inimigos_derrotados'];
    $response->moedas_acumuladas = $row['moedas_acumuladas'];
    $response->niveis_desbloqueados = $row['niveis_desbloqueados'];
    $response->status = "success";
} else {
    $response->erro = "Estatísticas não encontradas";
    $response->status = "fail";
}

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>
