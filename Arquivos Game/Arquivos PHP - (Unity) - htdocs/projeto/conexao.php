<?php
function conectarBanco() {
    $servidor = "localhost";
    $usuario = "root";
    $senha = "";
    $banco = "alunos_db";

    $conn = new mysqli($servidor, $usuario, $senha, $banco);

    if ($conn->connect_error) {
        die("Falha na conexão: " . $conn->connect_error);
    }
    return $conn;
}
?>
