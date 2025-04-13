<?php
include 'conexao.php'; // Arquivo de conexão com o banco

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $aluno_id = $_POST['aluno_id'];
    $progresso_nivel = $_POST['progresso_nivel'];
    $progresso_topico = $_POST['progresso_topico']; // Novo parâmetro

    $conn = conectarBanco();

    // Verifica se o nível já foi salvo para o tópico
    $sql = "SELECT * FROM progresso WHERE aluno_id = ? AND progresso_topico = ? AND progresso_nivel = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("isi", $aluno_id, $progresso_topico, $progresso_nivel);
    $stmt->execute();
    $result = $stmt->get_result();
    
    if ($result->num_rows == 0) {
        // Insere novo registro de progresso
        $sql = "INSERT INTO progresso (aluno_id, progresso_topico, progresso_nivel, progresso_concluido) VALUES (?, ?, ?, 1)";
        $stmt = $conn->prepare($sql);
        $stmt->bind_param("isi", $aluno_id, $progresso_topico, $progresso_nivel);
        if ($stmt->execute()) {
            echo json_encode(["status" => "sucesso"]);
        } else {
            echo json_encode(["status" => "erro"]);
        }
    } else {
        echo json_encode(["status" => "ja_registrado"]);
    }

    $stmt->close();
    $conn->close();
}
?>

