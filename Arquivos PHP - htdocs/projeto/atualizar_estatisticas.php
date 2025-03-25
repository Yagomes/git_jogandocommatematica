<?php
header('Content-Type: application/json');
ini_set('display_errors', 1);
error_reporting(E_ALL);

$conn = new mysqli('localhost', 'root', '', 'alunos_db');

if ($conn->connect_error) {
    die(json_encode(["status" => "fail", "erro" => "Erro de conexão: " . $conn->connect_error]));
}

// Verifica se os parâmetros foram recebidos corretamente
$dadosRecebidos = json_encode($_POST, JSON_PRETTY_PRINT);
file_put_contents("log.txt", $dadosRecebidos); // Cria um log para depuração

if (!isset($_POST['id_Aluno'], $_POST['total_jogado'], $_POST['acertos'], $_POST['erros'], 
          $_POST['inimigos_derrotados'], $_POST['moedas_acumuladas'], $_POST['niveis_desbloqueados'])) {
    echo json_encode(["status" => "fail", "erro" => "Parâmetros ausentes"]);
    exit();
}

$aluno_id = intval($_POST['id_Aluno']);
$total_jogado = intval($_POST['total_jogado']);
$acertos = intval($_POST['acertos']);
$erros = intval($_POST['erros']);
$inimigos_derrotados = intval($_POST['inimigos_derrotados']);
$moedas_acumuladas = intval($_POST['moedas_acumuladas']);
$niveis_desbloqueados = intval($_POST['niveis_desbloqueados']);

$sql = "INSERT INTO estatisticas (id_jogador, total_jogado, acertos, erros, inimigos_derrotados, moedas_acumuladas, niveis_desbloqueados)
VALUES (?, ?, ?, ?, ?, ?, ?)
ON DUPLICATE KEY UPDATE
total_jogado = VALUES(total_jogado),
acertos = VALUES(acertos),
erros = VALUES(erros),
inimigos_derrotados = VALUES(inimigos_derrotados),
moedas_acumuladas = VALUES(moedas_acumuladas),
niveis_desbloqueados = VALUES(niveis_desbloqueados);
";

$stmt = $conn->prepare($sql);

if (!$stmt) {
    echo json_encode(["status" => "fail", "erro" => "Erro na preparação da query: " . $conn->error]);
    exit();
}

$stmt->bind_param("iiiiiii", $aluno_id, $total_jogado, $acertos, $erros, $inimigos_derrotados, $moedas_acumuladas, $niveis_desbloqueados);

if ($stmt->execute()) {
    echo json_encode(["status" => "success"]);
} else {
    echo json_encode(["status" => "fail", "erro" => "Erro ao atualizar ou inserir: " . $stmt->error]);
}

$conn->close();
?>
