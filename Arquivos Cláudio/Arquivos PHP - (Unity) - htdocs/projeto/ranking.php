<?php
$servername = "localhost"; // Altere conforme necessário
$username = "root"; // Altere conforme necessário
$password = ""; // Altere conforme necessário
$dbname = "alunos_db"; // Altere conforme necessário

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

$turma_id = $_GET['turma_id']; // ID da turma do aluno logado

$sql = "SELECT aluno.aluno_nome, estatisticas.estatistica_moedas_acumuladas, estatisticas.estatistica_acertos, estatisticas.estatistica_inimigos_derrotados, estatisticas.estatistica_niveis_desbloqueados 
        FROM estatisticas
        JOIN aluno ON estatisticas.aluno_id = aluno.aluno_id
        WHERE aluno.turma_id = ?
        ORDER BY estatisticas.estatistica_moedas_acumuladas DESC";

$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $turma_id);
$stmt->execute();
$result = $stmt->get_result();

$ranking = [];
while ($row = $result->fetch_assoc()) {
    $ranking[] = $row;
}

echo json_encode($ranking);
$conn->close();
?>
