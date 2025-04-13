<?php
include 'conexao.php';

if ($_SERVER["REQUEST_METHOD"] == "GET") {
    $aluno_id = $_GET['aluno_id'];
    $progresso_topico = $_GET['progresso_topico']; // Novo parâmetro
    
    $conn = conectarBanco();

    // Seleciona o maior nível concluído para o tópico específico
    $sql = "SELECT MAX(progresso_nivel) AS progresso_nivel_max FROM progresso WHERE aluno_id = ? AND progresso_topico = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("is", $aluno_id, $progresso_topico);
    $stmt->execute();
    $result = $stmt->get_result();
    
    if ($row = $result->fetch_assoc()) {
        echo json_encode(["progresso_nivel_max" => $row["progresso_nivel_max"] ?? 1]);
    } else {
        echo json_encode(["progresso_nivel_max" => 1]); // Se não houver progresso, inicia no nível 1
    }

    $stmt->close();
    $conn->close();
}
?>
