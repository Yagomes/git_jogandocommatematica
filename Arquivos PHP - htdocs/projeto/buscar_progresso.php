<?php
include 'conexao.php';

if ($_SERVER["REQUEST_METHOD"] == "GET") {
    $aluno_id = $_GET['aluno_id'];
    $topico = $_GET['topico']; // Novo parâmetro
    
    $conn = conectarBanco();

    // Seleciona o maior nível concluído para o tópico específico
    $sql = "SELECT MAX(nivel) AS nivel_max FROM progresso WHERE aluno_id = ? AND topico = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("is", $aluno_id, $topico);
    $stmt->execute();
    $result = $stmt->get_result();
    
    if ($row = $result->fetch_assoc()) {
        echo json_encode(["nivel_max" => $row["nivel_max"] ?? 1]);
    } else {
        echo json_encode(["nivel_max" => 1]); // Se não houver progresso, inicia no nível 1
    }

    $stmt->close();
    $conn->close();
}
?>
