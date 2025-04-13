<?php
session_start();

// Verifica se o usuário está autenticado como administrador
if (!isset($_SESSION['adm_nome']) || empty($_SESSION['adm_nome'])) {
    header("Location: Login.php"); // Redireciona para o login se não estiver logado
    exit();
}

$adm_nome = $_SESSION['adm_nome'];
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Tela do Administrador</title>
    <link href="Tela_Adm.css" rel="stylesheet">
</head>

<body>
    <div class="container">
        <h2>Bem-vindo, Administrador <?php echo htmlspecialchars($adm_nome); ?></h2>
        <h3>O que você deseja fazer?</h3>

        <div class="options-container">
            <a href="Cadastrar Aluno/Aluno.php" class="option-button">Cadastrar Aluno</a>
            <a href="Cadastrar turma/Turma.php" class="option-button">Cadastrar Turma</a>
            <a href="Cadastrar Professor/Professor.php" class="option-button">Cadastrar Professor</a>
        </div>

        <!-- Botão de Voltar -->
        <div style="margin-top: 20px;">
            <a href="../Login/Login.php" class="button-red">Voltar</a>
        </div>
    </div>
</body>

</html>