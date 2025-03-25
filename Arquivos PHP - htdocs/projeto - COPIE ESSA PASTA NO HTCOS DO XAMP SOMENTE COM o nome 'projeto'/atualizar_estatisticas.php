<?php
$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die("Erro de conexão: " . $conn->connect_error);
}

$aluno_id = $_POST['id_Aluno'];
$total_jogado = $_POST['total_jogado'];
$acertos = $_POST['acertos'];
$erros = $_POST['erros'];
$inimigos_derrotados = $_POST['inimigos_derrotados'];
$moedas_acumuladas = $_POST['moedas_acumuladas'];

$response = new stdClass();

// Primeiro, tentamos inserir um novo registro ou atualizar se já existir
$sql =  "INSERT INTO estatisticas (id_jogador, total_jogado, acertos, erros, inimigos_derrotados, moedas_acumuladas) 
VALUES (?, ?, ?, ?, ?, ?) 
ON DUPLICATE KEY UPDATE 
total_jogado = VALUES(total_jogado), 
acertos = VALUES(acertos), 
erros = VALUES(erros), 
inimigos_derrotados = VALUES(inimigos_derrotados), 
moedas_acumuladas = VALUES(moedas_acumuladas)";


$stmt = $conn->prepare($sql);
$stmt->bind_param("iiiiii", $aluno_id, $total_jogado, $acertos, $erros, $inimigos_derrotados, $moedas_acumuladas);

if ($stmt->execute()) {
    $response->status = "success";
} else {
    $response->erro = "Erro ao atualizar ou inserir";
    $response->status = "fail";
}

header('Content-Type: application/json');
echo json_encode($response);

$conn->close();
?>
