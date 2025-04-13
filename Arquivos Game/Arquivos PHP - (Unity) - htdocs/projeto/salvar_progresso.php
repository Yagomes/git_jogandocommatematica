<?php
include 'conexao.php'; // Arquivo de conexão com o banco

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $aluno_id = $_POST['aluno_id'];
    $nivel = $_POST['nivel'];
    $topico = $_POST['topico']; // Novo parâmetro

    $conn = conectarBanco();

    // Verifica se o nível já foi salvo para o tópico
    $sql = "SELECT * FROM progresso WHERE aluno_id = ? AND topico = ? AND nivel = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("isi", $aluno_id, $topico, $nivel);
    $stmt->execute();
    $result = $stmt->get_result();
    
    if ($result->num_rows == 0) {
        // Insere novo registro de progresso
        $sql = "INSERT INTO progresso (aluno_id, topico, nivel, concluido) VALUES (?, ?, ?, 1)";
        $stmt = $conn->prepare($sql);
        $stmt->bind_param("isi", $aluno_id, $topico, $nivel);
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

