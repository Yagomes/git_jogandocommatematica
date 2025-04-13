<?php
// Conexão com o banco de dados
$conn = new mysqli("localhost", "root", "", "alunos_db");

// Verifica a conexão
if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

// Cadastrar um novo professor
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['cadastrar'])) {
    $matricula = $_POST['matricula'];
    $nome = $_POST['nome'];
    $senha = $_POST['senha'];

    // Insere o novo professor no banco de dados
    $sql = "INSERT INTO usuario (matricula, Nome, senha, cargo_usuario) VALUES ('$matricula', '$nome', '$senha', 'Professor')";

    if ($conn->query($sql) === TRUE) {
        echo "Professor cadastrado com sucesso!";
    } else {
        echo "Erro ao cadastrar professor: " . $conn->error;
    }
}

// Excluir um professor
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['delete_id'])) {
    $delete_id = $_POST['delete_id'];

    // Deleta o professor do banco de dados
    $sql = "DELETE FROM usuario WHERE id_usuario = '$delete_id'";

    if ($conn->query($sql) === TRUE) {
        //echo "Professor excluído com sucesso!";
    } else {
        echo "Erro ao excluir professor: " . $conn->error;
    }
}

// Buscar todos os professores cadastrados e as turmas que lecionam
$professores = $conn->query("SELECT usuario.id_usuario, usuario.matricula, usuario.Nome, GROUP_CONCAT(turma.nome SEPARATOR ', ') AS turmas 
                             FROM usuario
                             LEFT JOIN turma ON usuario.id_usuario = turma.id_prof
                             WHERE usuario.cargo_usuario = 'Professor'
                             GROUP BY usuario.id_usuario, usuario.matricula, usuario.Nome");

?>

<!DOCTYPE html>
<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cadastro de Professores</title>
    <link rel="stylesheet" href="professor.css">
    <script>
        function confirmarExclusao(id) {
            if (confirm('Tem certeza que deseja excluir este professor?')) {
                document.getElementById('delete_id').value = id;
                document.getElementById('deleteForm').submit();
            }
        }
    </script>
</head>

<body>
    <div class="container">
        <h1>Cadastro de Professores</h1>

        <a href="Criar Professor/Cadas_Professor.php" class="button">Cadastrar Novo Professor</a>

        <h2>Professores Cadastrados</h2>
        <!-- Tabela com todos os professores cadastrados -->
        <table>
            <thead>
                <tr>
                    <th>Matrícula</th>
                    <th>Nome</th>
                    <th>Turmas</th>
                    <th>Ação</th>
                </tr>
            </thead>
            <tbody>
                <?php
                // Exibe todos os professores cadastrados e as turmas que lecionam
                while ($professor = $professores->fetch_assoc()) {
                    echo "<tr>
                        <td>{$professor['matricula']}</td>
                        <td>{$professor['Nome']}</td>
                        <td>{$professor['turmas']}</td>
                        <td><button id='excluir' class='button' onclick='confirmarExclusao({$professor['id_usuario']})'>Excluir</button></td>
                    </tr>";
                }
                ?>
            </tbody>
        </table>

        <!-- Formulário oculto para exclusão -->
        <form method="post" action="" id="deleteForm">
            <input type="hidden" name="delete_id" id="delete_id">
        </form>
        <a href="../Tela_Adm.php"><button class="voltar">Voltar</button></a>
    </div>
</body>

</html>