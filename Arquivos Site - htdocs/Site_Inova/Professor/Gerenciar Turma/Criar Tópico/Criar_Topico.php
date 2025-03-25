<?php
session_start();

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "alunos_db";

// Cria a conexão
$conn = new mysqli($servername, $username, $password, $dbname);

// Verifica a conexão
if ($conn->connect_error) {
    die("Falha na conexão: " . $conn->connect_error);
}

// Verifica se o usuário está logado
if (!isset($_SESSION['usuario_id'])) {
    die("Usuário não está logado.");
}

// Inicializa mensagens de alerta
$alerta = "";
$alerta2 = "";
$alerta3 = "";

// Processa o envio do formulário de criar novo tópico
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $new_topico_nome = isset($_POST['new_topico_nome']) ? $_POST['new_topico_nome'] : '';
    $new_topico_min = isset($_POST['new_topico_min']) ? $_POST['new_topico_min'] : 0;
    $new_topico_max = isset($_POST['new_topico_max']) ? $_POST['new_topico_max'] : 0;

    // Verifica se os campos estão preenchidos
    if (!empty($new_topico_nome) && !empty($new_topico_min) && !empty($new_topico_max)) {
        // Validação específica para divisão
        if ($new_topico_nome == "div" && ($new_topico_min < 0 || $new_topico_max < 0)) {
            $alerta3 = "Erro: Para o tópico de divisão, os números não podem ser negativos.";
        }
        // Valida se o número mínimo é menor ou igual ao número máximo
        else if ($new_topico_min <= $new_topico_max) {
            $sql = "INSERT INTO topico (Nome_topico, Num_Min_topico, Num_Max_topico) VALUES ('$new_topico_nome', '$new_topico_min', '$new_topico_max')";
            if ($conn->query($sql) === TRUE) {
                $alerta = "Novo tópico criado com sucesso!";
            } else {
                $alerta2 = "Erro ao criar novo tópico: " . $conn->error;
            }
        } else {
            $alerta3 = "Erro: O Número Mínimo deve ser menor ou igual ao Número Máximo.";
        }
    } else {
        $alerta3 = "Por favor, preencha todos os campos para criar um novo tópico.";
    }
}

$conn->close();
?>

<!DOCTYPE html>
<html>

<head>
    <title>Criar Novo Tópico</title>
    <link rel="stylesheet" type="text/css" href="Criar_Topico.css">
</head>

<body>
    <div class="container">
        <h1>Criar Novo Tópico</h1>

        <!-- Mensagem de alerta -->
        <?php if (!empty($alerta)): ?>
            <p class="alert"><?php echo $alerta; ?></p>
        <?php endif; ?>
        <?php if (!empty($alerta2)): ?>
            <p class="alert2"><?php echo $alerta2; ?></p>
        <?php endif; ?>
        <?php if (!empty($alerta3)): ?>
            <p class="alert3"><?php echo $alerta3; ?></p>
        <?php endif; ?>

        <!-- Formulário para criar novo tópico -->
        <form method="post" action="">
            <label for="new_topico_nome">Nome do Tópico:</label>
            <select id="new_topico_nome" name="new_topico_nome" required>
                <option value="">Selecione</option>
                <option value="soma">soma</option>
                <option value="mult">multiplicação</option>
                <option value="div">divisão</option>
                <option value="sub">subtração</option>
            </select><br><br>

            <label for="new_topico_min">Número Mínimo:</label>
            <input type="number" id="new_topico_min" name="new_topico_min" required><br><br>
            <label for="new_topico_max">Número Máximo:</label>
            <input type="number" id="new_topico_max" name="new_topico_max" required><br><br>

            <button type="submit">Criar Novo Tópico</button>
        </form>

        <br>
        <a href="../G_Turma/Geren_Turma.php"><button class="voltar">Voltar</button></a>
    </div>
</body>

</html>