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

if (!isset($_POST['aluno_id'], $_POST['estatistica_total_jogado'], $_POST['estatistica_acertos'], $_POST['estatistica_erros'], 
          $_POST['estatistica_inimigos_derrotados'], $_POST['estatistica_moedas_acumuladas'], $_POST['estatistica_niveis_desbloqueados'])) {
    echo json_encode(["status" => "fail", "erro" => "Parâmetros ausentes"]);
    exit();
}

$aluno_id = intval($_POST['aluno_id']);
$estatistica_total_jogado = intval($_POST['estatistica_total_jogado']);
$estatistica_acertos = intval($_POST['estatistica_acertos']);
$estatistica_erros = intval($_POST['estatistica_erros']);
$estatistica_inimigos_derrotados = intval($_POST['estatistica_inimigos_derrotados']);
$estatistica_moedas_acumuladas = intval($_POST['estatistica_moedas_acumuladas']);
$estatistica_niveis_desbloqueados = intval($_POST['estatistica_niveis_desbloqueados']);

$sql = "INSERT INTO estatisticas (aluno_id, estatistica_total_jogado, estatistica_acertos, estatistica_erros, estatistica_inimigos_derrotados, estatistica_moedas_acumuladas, estatistica_niveis_desbloqueados)
VALUES (?, ?, ?, ?, ?, ?, ?)
ON DUPLICATE KEY UPDATE
estatistica_total_jogado = VALUES(estatistica_total_jogado),
estatistica_acertos = VALUES(estatistica_acertos),
estatistica_erros = VALUES(estatistica_erros),
estatistica_inimigos_derrotados = VALUES(estatistica_inimigos_derrotados),
estatistica_moedas_acumuladas = VALUES(estatistica_moedas_acumuladas),
estatistica_niveis_desbloqueados = VALUES(estatistica_niveis_desbloqueados);
";

$stmt = $conn->prepare($sql);

if (!$stmt) {
    echo json_encode(["status" => "fail", "erro" => "Erro na preparação da query: " . $conn->error]);
    exit();
}

$stmt->bind_param("iiiiiii", $aluno_id, $estatistica_total_jogado, $estatistica_acertos, $estatistica_erros, $estatistica_inimigos_derrotados, $estatistica_moedas_acumuladas, $estatistica_niveis_desbloqueados);

if ($stmt->execute()) {
    echo json_encode(["status" => "success"]);
} else {
    echo json_encode(["status" => "fail", "erro" => "Erro ao atualizar ou inserir: " . $stmt->error]);
}

$conn->close();
?>
