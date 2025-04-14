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
    $aluno_matricula = $_POST['aluno_matricula'];
    $aluno_nome = $_POST['aluno_nome'];
    $aluno_senha = $_POST['aluno_senha'];
    $aluno_genero = $_POST['aluno_genero'];
    $turma_id = $_POST['turma_id'];

    // Verifica se a matrícula já existe
    $sql_check = "SELECT * FROM aluno WHERE aluno_matricula = ?";
    if ($stmt_check = $conn->prepare($sql_check)) {
        $stmt_check->bind_param("s", $aluno_matricula);
        $stmt_check->execute();
        $stmt_check->store_result();

        if ($stmt_check->num_rows > 0) {
            $_SESSION['mensagem'] = "Esta matrícula já está cadastrada!";
            $_SESSION['tipo_mensagem'] = "alert-error"; // Mensagem de erro
        } else {
            // Prepara e executa a consulta de inserção
            $sql = "INSERT INTO aluno (aluno_matricula, aluno_nome, aluno_senha, aluno_genero, turma_id) VALUES (?, ?, ?, ?, ?)";
            if ($stmt = $conn->prepare($sql)) {
                $stmt->bind_param("ssssi", $aluno_matricula, $aluno_nome, $aluno_senha, $aluno_genero, $turma_id);

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
            <label for="aluno_matricula">Matrícula:</label>
            <input type="text" name="aluno_matricula" required><br>
            <label for="aluno_nome">Nome:</label>
            <input type="text" name="aluno_nome" required><br>
            <label for="aluno_senha">Senha:</label>
            <input type="password" name="aluno_senha" required><br>
            <label for="turma_id">Turma:</label>
            <select name="turma_id" required>
                <option value="">Selecione</option>
                <?php
                // Consulta para obter as turmas do banco de dados
                $sql = "SELECT turma_id, turma_nome FROM turma";
                $result = $conn->query($sql);

                if ($result->num_rows > 0) {
                    while ($row = $result->fetch_assoc()) {
                        echo "<option value='" . $row['turma_id'] . "'>" . $row['turma_nome'] . "</option>";
                    }
                } else {
                    echo "<option value=''>Nenhuma turma encontrada</option>";
                }
                ?>
            </select><br>
            <label for="aluno_genero">Gênero:</label>
            <select name="aluno_genero" required>
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