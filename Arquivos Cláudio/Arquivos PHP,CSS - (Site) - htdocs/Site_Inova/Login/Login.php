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
    if (isset($_POST['matricula']) && isset($_POST['senha'])) {
        $matricula = $_POST['matricula'];
        $senha = $_POST['senha'];

        $stmt = $conn->prepare("SELECT id_usuario, cargo_usuario, Nome FROM usuario WHERE matricula = ? AND senha = ?");
        $stmt->bind_param("ss", $matricula, $senha);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($result->num_rows > 0) {
            $row = $result->fetch_assoc();
            $cargo = $row['cargo_usuario'];
            $id_usuario = $row['id_usuario'];
            $nome = $row['Nome'];

            // Armazene as informações do usuário na sessão
            $_SESSION['usuario_id'] = $id_usuario;
            $_SESSION['usuario_nome'] = $nome;
            $_SESSION['usuario_cargo'] = $cargo;

            // Redirecione de acordo com o cargo
            if ($cargo === "adm") {
                $_SESSION['adm_nome'] = $nome; // Armazena o nome do administrador
                header("Location: ../Adm/Tela_Adm.php");
                exit();
            } elseif ($cargo === "Professor") {
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
            <label for="matricula">Matrícula:</label>
            <input type="text" id="matricula" name="matricula" required>

            <label for="senha">Senha:</label>
            <input type="password" id="senha" name="senha" required>

            <input type="submit" value="Entrar">
        </form>
    </div>
</body>

</html>