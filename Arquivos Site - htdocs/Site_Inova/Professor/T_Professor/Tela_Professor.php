<?php
session_start();

// Verifica se o usuário está logado e é um professor
if (!isset($_SESSION['usuario_id']) || $_SESSION['usuario_cargo'] !== "Professor") {
    header("Location: Login.php");
    exit();
}

$professor_nome = $_SESSION['usuario_nome'];

// Configuração do banco de dados
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "alunos_db";

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Falha na conexão: " . $conn->connect_error);
}

// Busca todas as turmas que o professor leciona
$stmt = $conn->prepare("
    SELECT t.Nome AS turma, t.Serie AS serie
    FROM Turma t
    INNER JOIN Usuario u ON t.id_prof = u.id_usuario
    WHERE u.Nome = ?
");
$stmt->bind_param("s", $professor_nome);
$stmt->execute();
$result = $stmt->get_result();

$turmas = [];
while ($row = $result->fetch_assoc()) {
    $turmas[] = $row;
}

$stmt->close();
$conn->close();
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Tela do Professor</title>
    <link href="Tela_Professor.css" rel="stylesheet">
</head>

<body>
    <div class="container">
        <h2>Bem-vindo, Professor <?php echo htmlspecialchars($professor_nome); ?></h2>

        <!-- Botão Configurações da Turma -->
        <div style="margin-bottom: 15px;">
            <a href="../Gerenciar Turma/G_Turma/Geren_Turma.php">
                <button>Configurações da Turma</button>
            </a>
        </div>

        <h3>Suas Turmas</h3>

        <?php if (count($turmas) > 0): ?>
            <table>
                <thead>
                    <tr>
                        <th>Nome da Turma</th>
                        <th>Série</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($turmas as $turma): ?>
                        <tr>
                            <td><?php echo htmlspecialchars($turma['turma']); ?></td>
                            <td><?php echo htmlspecialchars($turma['serie']); ?></td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        <?php else: ?>
            <p>Você não está lecionando em nenhuma turma no momento.</p>
        <?php endif; ?>

        <!-- Botão Estatísticas -->
        <div style="margin-top: 15px;">
            <a href="../Estatísticas Turma/Est_Turma.php">
                <button>Estatísticas</button>
            </a>
        </div>

        <!-- Botão de Voltar -->
        <a href="../../Login/Login.php" class="button-red">Voltar</a>
    </div>
</body>

</html>