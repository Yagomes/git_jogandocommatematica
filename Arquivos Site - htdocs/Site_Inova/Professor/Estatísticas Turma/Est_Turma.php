<?php
session_start();

// Verifica se o usuário está logado e é um professor
if (!isset($_SESSION['usuario_id']) || $_SESSION['usuario_cargo'] !== "Professor") {
    header("Location: Login.php");
    exit();
}

$professor_nome = $_SESSION['usuario_nome'];

// Conexão com o banco de dados
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "alunos_db";

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Falha na conexão: " . $conn->connect_error);
}

// Busca as turmas que o professor leciona
$stmt = $conn->prepare("
    SELECT id_turma, nome, serie
    FROM turma
    WHERE id_prof = (SELECT id_usuario FROM usuario WHERE Nome = ?)
");
$stmt->bind_param("s", $professor_nome);
$stmt->execute();
$result = $stmt->get_result();

$turmas = [];
while ($row = $result->fetch_assoc()) {
    $turmas[] = $row;
}

$stmt->close();

// Prepara os dados dos alunos por turma
$dados_turmas = [];
foreach ($turmas as $turma) {
    $stmt = $conn->prepare("
        SELECT a.matricula, a.Nome AS aluno_nome, e.acertos, e.erros, e.total_jogado
        FROM aluno a
        INNER JOIN estatisticas e ON a.id_Aluno = e.id_jogador
        WHERE a.id_turma = ?
    ");
    $stmt->bind_param("i", $turma['id_turma']);
    $stmt->execute();
    $result = $stmt->get_result();

    $dados_alunos = [];
    while ($row = $result->fetch_assoc()) {
        $dados_alunos[] = $row;
    }

    $dados_turmas[] = [
        'turma_nome' => $turma['nome'],
        'serie' => $turma['serie'],
        'alunos' => $dados_alunos
    ];

    $stmt->close();
}

$conn->close();
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Estatísticas das Turmas</title>
    <link href="Est_Turma.css" rel="stylesheet">
</head>

<body>
    <div class="container">
        <h2>Estatísticas das Turmas</h2>

        <?php foreach ($dados_turmas as $dados_turma): ?>
            <h3><?php echo htmlspecialchars($dados_turma['turma_nome']) . " - " . htmlspecialchars($dados_turma['serie']); ?>
            </h3>
            <table>
                <thead>
                    <tr>
                        <th>Matrícula</th>
                        <th>Nome</th>
                        <th>Acertos</th>
                        <th>Erros</th>
                        <th>Total Jogado</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($dados_turma['alunos'] as $aluno): ?>
                        <tr>
                            <td><?php echo htmlspecialchars($aluno['matricula']); ?></td>
                            <td><?php echo htmlspecialchars($aluno['aluno_nome']); ?></td>
                            <td><?php echo htmlspecialchars($aluno['acertos']); ?></td>
                            <td><?php echo htmlspecialchars($aluno['erros']); ?></td>
                            <td><?php echo htmlspecialchars($aluno['total_jogado']); ?></td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        <?php endforeach; ?>

        <!-- Botão Voltar -->
        <a href="../T_Professor/Tela_Professor.php" class="button-red">Voltar</a>
    </div>
</body>

</html>