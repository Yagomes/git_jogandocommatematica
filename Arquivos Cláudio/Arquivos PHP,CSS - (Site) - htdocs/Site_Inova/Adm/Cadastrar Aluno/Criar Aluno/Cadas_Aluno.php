<?php
session_start();

// Conexão com o banco de dados
$conn = new mysqli("localhost", "root", "", "alunos_db");

// Verifica a conexão
if ($conn->connect_error) {
    die("Falha na conexão: " . $conn->connect_error);
}

// Verifica se o formulário foi submetido
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Coleta os dados do formulário
    $matricula = $_POST['matricula'];
    $nome = $_POST['nome'];
    $senha = $_POST['senha'];
    $genero = $_POST['genero'];
    $id_turma = $_POST['id_turma'];

    // Verifica se a matrícula já existe
    $sql_check = "SELECT * FROM aluno WHERE matricula = ?";
    if ($stmt_check = $conn->prepare($sql_check)) {
        $stmt_check->bind_param("s", $matricula);
        $stmt_check->execute();
        $stmt_check->store_result();

        if ($stmt_check->num_rows > 0) {
            $_SESSION['mensagem'] = "Esta matrícula já está cadastrada!";
            $_SESSION['tipo_mensagem'] = "alert-error"; // Mensagem de erro
        } else {
            // Prepara e executa a consulta de inserção
            $sql = "INSERT INTO aluno (matricula, Nome, senha, genero, id_turma) VALUES (?, ?, ?, ?, ?)";
            if ($stmt = $conn->prepare($sql)) {
                $stmt->bind_param("ssssi", $matricula, $nome, $senha, $genero, $id_turma);

                if ($stmt->execute()) {
                    $_SESSION['mensagem'] = "Aluno cadastrado com sucesso!";
                    $_SESSION['tipo_mensagem'] = "alert-success"; // Mensagem de sucesso
                } else {
                    $_SESSION['mensagem'] = "Erro ao cadastrar o aluno.";
                    $_SESSION['tipo_mensagem'] = "alert-error"; // Mensagem de erro
                }

                // Fecha a declaração
                $stmt->close();
            } else {
                $_SESSION['mensagem'] = "Erro na preparação da consulta: " . $conn->error;
                $_SESSION['tipo_mensagem'] = "alert-error"; // Mensagem de erro
            }
        }

        // Fecha a declaração de verificação
        $stmt_check->close();
    } else {
        $_SESSION['mensagem'] = "Erro na preparação da consulta de verificação: " . $conn->error;
        $_SESSION['tipo_mensagem'] = "alert-error"; // Mensagem de erro
    }

    // Fecha a conexão
    $conn->close();

    // Redireciona para a página de cadastro
    header("Location: cadas_aluno.php");
    exit();
}
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cadastrar Aluno</title>
    <link rel="stylesheet" href="Cadas_Aluno.css">
</head>

<body>
    <div class="container">
        <h2>Cadastrar Aluno</h2>

        <?php
        if (isset($_SESSION['mensagem'])) {
            echo "<p class='" . $_SESSION['tipo_mensagem'] . "'>" . $_SESSION['mensagem'] . "</p>";
            unset($_SESSION['mensagem'], $_SESSION['tipo_mensagem']);
        }
        ?>

        <form action="cadas_aluno.php" method="post">
            <label for="matricula">Matrícula:</label>
            <input type="text" name="matricula" required><br>
            <label for="nome">Nome:</label>
            <input type="text" name="nome" required><br>
            <label for="senha">Senha:</label>
            <input type="password" name="senha" required><br>
            <label for="id_turma">Turma:</label>
            <select name="id_turma" required>
                <option value="">Selecione</option>
                <?php
                // Consulta para obter as turmas do banco de dados
                $sql = "SELECT id_turma, nome FROM turma";
                $result = $conn->query($sql);

                if ($result->num_rows > 0) {
                    while ($row = $result->fetch_assoc()) {
                        echo "<option value='" . $row['id_turma'] . "'>" . $row['nome'] . "</option>";
                    }
                } else {
                    echo "<option value=''>Nenhuma turma encontrada</option>";
                }
                ?>
            </select><br>
            <label for="genero">Gênero:</label>
            <select name="genero" required>
                <option value="">Selecione</option>
                <option value="Masculino">Masculino</option>
                <option value="Feminino">Feminino</option>
            </select><br>
            <button type="submit" class="large-button">Cadastrar Aluno</button>
        </form>

        <a href="../Aluno.php" class="back-button">Voltar</a>
    </div>
</body>

</html>