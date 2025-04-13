<?php
$servername = "localhost"; // Altere conforme necessário
$username = "root"; // Altere conforme necessário
$password = ""; // Altere conforme necessário
$dbname = "alunos_db"; // Altere conforme necessário

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

$id_turma = $_GET['id_turma']; // ID da turma do aluno logado

$sql = "SELECT aluno.Nome, estatisticas.moedas_acumuladas, estatisticas.acertos, estatisticas.inimigos_derrotados, estatisticas.niveis_desbloqueados 
        FROM estatisticas
        JOIN aluno ON estatisticas.id_jogador = aluno.id_Aluno
        WHERE aluno.id_turma = ?
        ORDER BY estatisticas.moedas_acumuladas DESC";

$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $id_turma);
$stmt->execute();
$result = $stmt->get_result();

$ranking = [];
while ($row = $result->fetch_assoc()) {
    $ranking[] = $row;
}

echo json_encode($ranking);
$conn->close();
?>
