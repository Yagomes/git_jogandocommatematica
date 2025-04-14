<?php
session_start();

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "alunos_db";

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Falha na conexão: " . $conn->connect_error);
}

$mensagem = '';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['usuario_matricula']) && isset($_POST['usuario_senha'])) {
        $usuario_matricula = $_POST['usuario_matricula'];
        $usuario_senha = $_POST['usuario_senha'];

        $stmt = $conn->prepare("SELECT usuario_id, usuario_cargo, usuario_nome FROM usuario WHERE usuario_matricula = ? AND usuario_senha = ?");
        $stmt->bind_param("ss", $usuario_matricula, $usuario_senha);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($result->num_rows > 0) {
            $row = $result->fetch_assoc();
            $usuario_cargo = $row['usuario_cargo'];
            $usuario_id = $row['usuario_id'];
            $usuario_nome = $row['usuario_nome'];

            // Armazene as informações do usuário na sessão
            $_SESSION['usuario_id'] = $usuario_id;
            $_SESSION['usuario_nome'] = $usuario_nome;
            $_SESSION['usuario_cargo'] = $usuario_cargo;

            // Redirecione de acordo com o cargo
            if ($usuario_cargo === "adm") {
                $_SESSION['adm_nome'] = $usuario_nome; // Armazena o nome do administrador
                header("Location: ../Adm/Tela_Adm.php");
                exit();
            } elseif ($usuario_cargo === "Professor") {
                header("Location: ../Professor/T_Professor/Tela_Professor.php");
                exit();
            }
        } else {
            $_SESSION['mensagem'] = "<span class='alert'>Usuário não encontrado. Verifique seus dados e tente novamente.</span>";
        }
        $stmt->close();
    }
}

if (isset($_SESSION['mensagem'])) {
    $mensagem = $_SESSION['mensagem'];
    unset($_SESSION['mensagem']);
}

$conn->close();
?>


<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <link href="Login.css" rel="stylesheet">
</head>

<body>
    <div class="login-container">
        <h2>Login</h2>
        <?php if (!empty($mensagem)): ?>
            <?php echo $mensagem; ?>
        <?php endif; ?>
        <form action="" method="post">
            <label for="usuario_matricula">Matrícula:</label>
            <input type="text" id="usuario_matricula" name="usuario_matricula" required>

            <label for="usuario_senha">Senha:</label>
            <input type="password" id="usuario_senha" name="usuario_senha" required>

            <input type="submit" value="Entrar">
        </form>
    </div>
</body>

</html>