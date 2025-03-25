<?php
session_start();

// Verifica se o usuário está autenticado como administrador
if (!isset($_SESSION['adm_nome']) || empty($_SESSION['adm_nome'])) {
    header("Location: Login.php"); // Redireciona para o login se não estiver logado
    exit();
}

$adm_nome = $_SESSION['adm_nome'];

// Conexão com o banco de dados
$conn = new mysqli("localhost", "root", "", "alunos_db");

// Verifica a conexão
if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

// Verifica se o formulário foi submetido
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Coleta os dados do formulário
    $nome = $_POST['nome'];
    $matricula = $_POST['matricula'];
    $senha = $_POST['senha'];
    $cargo_usuario = 'Professor';

    // Verifica se a matrícula já existe
    $sql_check = "SELECT * FROM usuario WHERE matricula = ?";
    if ($stmt_check = $conn->prepare($sql_check)) {
        $stmt_check->bind_param("s", $matricula);
        $stmt_check->execute();
        $stmt_check->store_result();

        if ($stmt_check->num_rows > 0) {
            $_SESSION['mensagem'] = "Esta matrícula já está cadastrada!";
            $_SESSION['tipo_mensagem'] = "alert-error"; // Classe para erro
        } else {
            // Prepara e executa a consulta de inserção
            $sql = "INSERT INTO usuario (Nome, matricula, senha, cargo_usuario) VALUES (?, ?, ?, ?)";
            if ($stmt = $conn->prepare($sql)) {
                $stmt->bind_param("ssss", $nome, $matricula, $senha, $cargo_usuario);

                if ($stmt->execute()) {
                    $_SESSION['mensagem'] = "Professor cadastrado com sucesso!";
                    $_SESSION['tipo_mensagem'] = "alert-success"; // Classe para sucesso
                } else {
                    $_SESSION['mensagem'] = "Erro ao cadastrar professor.";
                    $_SESSION['tipo_mensagem'] = "alert-error"; // Classe para erro
                }

                // Fecha a declaração
                $stmt->close();
            } else {
                $_SESSION['mensagem'] = "Erro na preparação da consulta: " . $conn->error;
                $_SESSION['tipo_mensagem'] = "alert-error"; // Classe para erro
            }
        }

        // Fecha a declaração de verificação
        $stmt_check->close();
    } else {
        $_SESSION['mensagem'] = "Erro na preparação da consulta de verificação: " . $conn->error;
        $_SESSION['tipo_mensagem'] = "alert-error"; // Classe para erro
    }

    // Fecha a conexão
    $conn->close();

    // Redireciona para a página de cadastro
    header("Location: cadas_professor.php");
    exit();
}
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cadastro de Professor</title>
    <link href="cadas_professor.css" rel="stylesheet">
</head>

<body>
    <div class="container">
        <h2>Cadastro de Professor</h2>

        <?php
        if (isset($_SESSION['mensagem'])) {
            echo "<p class='" . $_SESSION['tipo_mensagem'] . "'>" . $_SESSION['mensagem'] . "</p>";
            unset($_SESSION['mensagem'], $_SESSION['tipo_mensagem']);
        }
        ?>

        <form action="cadas_professor.php" method="post">
            <label for="nome">Nome:</label>
            <input type="text" id="nome" name="nome" required><br>
            <label for="matricula">Matrícula:</label>
            <input type="text" id="matricula" name="matricula" required><br>
            <label for="senha">Senha:</label>
            <input type="password" id="senha" name="senha" required><br>
            <input type="hidden" id="cargo_usuario" name="cargo_usuario" value="Professor">
            <button type="submit" class="large-button">Cadastrar</button>
        </form>
        <br>
        <a href="../Professor.php" class="back-button">Voltar</a>
    </div>
</body>

</html>