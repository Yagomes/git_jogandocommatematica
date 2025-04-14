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
    $turma_nome = $_POST['turma_nome'];
    $usuario_id = isset($_POST['usuario_id']) ? $_POST['usuario_id'] : NULL;
    $turma_serie = isset($_POST['turma_serie']) ? $_POST['turma_serie'] : NULL;

    // Prepara e executa a consulta de inserção
    $sql = "INSERT INTO turma (turma_nome, usuario_id, turma_serie) VALUES (?, ?, ?)";
    if ($stmt = $conn->prepare($sql)) {
        $stmt->bind_param("sis", $turma_nome, $usuario_id, $turma_serie);

        if ($stmt->execute()) {
            $_SESSION['mensagem'] = "Turma cadastrada com sucesso!!!";
        } else {
            $_SESSION['mensagem'] = "Erro ao cadastrar a turma: " . $stmt->error;
        }

        // Fecha a declaração
        $stmt->close();
    } else {
        $_SESSION['mensagem'] = "Erro na preparação da consulta: " . $conn->error;
    }

    // Fecha a conexão
    $conn->close();

    // Redireciona para a página de cadastro
    header("Location: cadas_turma.php");
    exit();
}

// Consulta para obter os professores do banco de dados
$professores = $conn->query("SELECT usuario_id, usuario_nome FROM usuario WHERE usuario_cargo = 'Professor'");
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cadastrar Turma</title>
    <link rel="stylesheet" href="Cadas_Turma.css">
</head>

<body>
    <div class="container">
        <h2>Cadastrar Turma</h2>

        <?php
        if (isset($_SESSION['mensagem'])) {
            echo "<p class='alert'>" . $_SESSION['mensagem'] . "</p>";
            unset($_SESSION['mensagem']);
        }
        ?>

        <form action="cadas_turma.php" method="post">
            <label for="turma_nome">Nome:</label>
            <input type="text" name="turma_nome" required><br>

            <label for="usuario_id">Professor:</label>
            <select name="usuario_id" required>
                <option value="">Selecione</option>
                <?php
                while ($professor = $professores->fetch_assoc()) {
                    echo "<option value='" . $professor['usuario_id'] . "'>" . $professor['usuario_nome'] . "</option>";
                }
                ?>
            </select><br>

            <label for="turma_serie">Série:</label>
            <select name="turma_serie" required>
                <option value="">Selecione</option>
                <option value="1º Ano do Fundamental">1º Ano do Fundamental</option>
                <option value="2º Ano do Fundamental">2º Ano do Fundamental</option>
                <option value="3º Ano do Fundamental">3º Ano do Fundamental</option>
                <option value="4º Ano do Fundamental">4º Ano do Fundamental</option>
                <option value="5º Ano do Fundamental">5º Ano do Fundamental</option>
                <option value="6º Ano do Fundamental">6º Ano do Fundamental</option>
                <option value="7º Ano do Fundamental">7º Ano do Fundamental</option>
                <option value="8º Ano do Fundamental">8º Ano do Fundamental</option>
                <option value="9º Ano do Fundamental">9º Ano do Fundamental</option>
                <option value="1º Ano do Ensino Medio">1º Ano do Ensino Medio</option>
                <option value="2º Ano do Ensino Medio">2º Ano do Ensino Medio</option>
                <option value="3º Ano do Ensino Medio">3º Ano do Ensino Medio</option>
            </select><br>

            <button type="submit" class="large-button">Cadastrar Turma</button>
        </form>

        <!-- Botão para voltar ao menu principal -->
        <a href="../Turma.php" class="back-button">Voltar</a>
    </div>
</body>

</html>