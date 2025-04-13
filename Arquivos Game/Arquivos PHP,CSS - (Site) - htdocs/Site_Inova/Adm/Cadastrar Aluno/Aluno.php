<?php
session_start();
// Verifica se o usuário está autenticado como administrador
if (!isset($_SESSION['adm_nome']) || empty($_SESSION['adm_nome'])) {
    header("Location: Login.php"); // Redireciona para o login se não estiver logado
    exit();
}

$adm_nome = $_SESSION['adm_nome'];

// Conexão com o banco de dados
$servername = "localhost"; // Ajuste conforme sua configuração
$username = "root"; // Ajuste conforme sua configuração
$password = ""; // Ajuste conforme sua configuração
$dbname = "alunos_db"; // Nome do banco de dados

$conn = new mysqli($servername, $username, $password, $dbname);

// Verifica a conexão
if ($conn->connect_error) {
    die("Connection failed:" . $conn->connect_error);
}

// Excluir um aluno
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['delete_id'])) {
    $delete_id = $_POST['delete_id'];

    // Deleta o aluno do banco de dados
    $sql = "DELETE FROM aluno WHERE id_Aluno = '$delete_id'";

    if ($conn->query($sql) === TRUE) {
        echo "Aluno excluído com sucesso!";
    } else {
        echo "Erro ao excluir aluno: " . $conn->error;
    }
}

// Obtendo todos os alunos cadastrados
$sql = "SELECT * FROM aluno";
$result = $conn->query($sql);
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Alunos Cadastrados</title>
    <link rel="stylesheet" href="Aluno.css">
    <script>
        function confirmarExclusao(id) {
            if (confirm('Tem certeza que deseja excluir este aluno?')) {
                document.getElementById('delete_id').value = id;
                document.getElementById('deleteForm').submit();
            }
        }
    </script>
</head>

<body>
    <div class="container">
        <h1>Cadastro de Alunos</h1>
        <a href="Criar Aluno/Cadas_Aluno.php" class="button">Cadastrar Novo Aluno</a>
        <h2>Alunos Cadastrados</h2>

        <!-- Tabela com alunos -->
        <table cellpadding="10">
            <thead>
                <tr>
                    <th>Matrícula</th>
                    <th>Nome</th>
                    <th>Gênero</th>
                    <th>Turma</th>
                    <th>Ação</th>
                </tr>
            </thead>
            <tbody>
                <?php
                if ($result->num_rows > 0) {
                    // Exibe os dados de cada aluno
                    while ($row = $result->fetch_assoc()) {
                        // Obtendo o nome da turma
                        $turma_sql = "SELECT nome FROM turma WHERE id_turma = " . $row['id_turma'];
                        $turma_result = $conn->query($turma_sql);
                        $turma_nome = $turma_result->fetch_assoc()['nome'];
                        ?>
                        <tr>
                            <td><?php echo $row['matricula']; ?></td>
                            <td><?php echo $row['Nome']; ?></td>
                            <td><?php echo $row['genero']; ?></td>
                            <td><?php echo $turma_nome; ?></td>
                            <td><button id="excluir" class="button"
                                    onclick="confirmarExclusao(<?php echo $row['id_Aluno']; ?>)">Excluir</button></td>
                        </tr>
                        <?php
                    }
                } else {
                    echo "<tr><td colspan='5'>Nenhum aluno encontrado</td></tr>";
                }
                ?>
            </tbody>
        </table>

        <!-- Formulário oculto para exclusão -->
        <form method="post" action="" id="deleteForm">
            <input type="hidden" name="delete_id" id="delete_id">
        </form>
        <a href="../Tela_Adm.php" class="back-button">Voltar</a>
    </div>
    <?php
    // Fecha a conexão com o banco de dados
    $conn->close();
    ?>
</body>

</html>