<?php
// Conexão com o banco de dados
$conn = new mysqli("localhost", "root", "", "alunos_db");

// Verifica a conexão
if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

// Excluir uma turma
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['delete_id'])) {
    $delete_id = $_POST['delete_id'];

    // Deleta a turma do banco de dados
    $sql = "DELETE FROM turma WHERE turma_id = '$delete_id'";

    if ($conn->query($sql) === TRUE) {
        //echo "Turma excluída com sucesso!";
    } else {
        echo "Erro ao excluir turma: " . $conn->error;
    }
}

// Busca todas as turmas cadastradas
$turmas = $conn->query("SELECT t.turma_id, t.turma_nome, u.usuario_nome AS professor, t.turma_serie 
                        FROM turma t
                        LEFT JOIN usuario u ON t.usuario_id = u.usuario_id");
?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Listagem de Turmas</title>
    <link rel="stylesheet" href="turma.css">
    <script>
        function confirmarExclusao(id) {
            if (confirm('Tem certeza que deseja excluir esta turma?')) {
                document.getElementById('delete_id').value = id;
                document.getElementById('deleteForm').submit();
            }
        }
    </script>
</head>

<body>
    <div class="container">
        <h1>Cadastro de Turmas</h1>
        <a href="Criar Turma/Cadas_Turma.php" class="button">Cadastrar Nova Turma</a>
        <!-- Listagem de turmas -->
        <h2>Turmas Cadastradas</h2>
        <table>
            <thead>
                <tr>
                    <th>Nome</th>
                    <th>Professor</th>
                    <th>Série</th>
                    <th>Ação</th>
                </tr>
            </thead>
            <tbody>
                <?php
                // Exibe todas as turmas cadastradas
                while ($turma = $turmas->fetch_assoc()) {
                    echo "<tr>
                        <td>{$turma['turma_nome']}</td>
                        <td>{$turma['professor']}</td>
                        <td>{$turma['turma_serie']}</td>
                        <td><button id='excluir' class='button excluir' onclick='confirmarExclusao({$turma['turma_id']})'>Excluir</button></td>
                    </tr>";
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
</body>

</html>