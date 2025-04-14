<?php
// Conexão com o banco de dados
$conn = new mysqli("localhost", "root", "", "alunos_db");

// Verifica a conexão
if ($conn->connect_error) {
    die("Conexão falhou: " . $conn->connect_error);
}

// Cadastrar um novo professor
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['cadastrar'])) {
    $usuario_matricula = $_POST['usuario_matricula'];
    $usuario_nome = $_POST['usuario_nome'];
    $usuario_senha = $_POST['usuario_senha'];

    // Insere o novo professor no banco de dados
    $sql = "INSERT INTO usuario (usuario_matricula, usuario_nome, usuario_senha, usuario_cargo) VALUES ('$usuario_matricula', '$usuario_nome', '$usuario_senha', 'Professor')";

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
    $sql = "DELETE FROM usuario WHERE usuario_id = '$delete_id'";

    if ($conn->query($sql) === TRUE) {
        //echo "Professor excluído com sucesso!";
    } else {
        echo "Erro ao excluir professor: " . $conn->error;
    }
}

// Buscar todos os professores cadastrados e as turmas que lecionam
$professores = $conn->query("SELECT usuario.usuario_id, usuario.usuario_matricula, usuario.usuario_nome, GROUP_CONCAT(turma.turma_nome SEPARATOR ', ') AS turmas 
                             FROM usuario
                             LEFT JOIN turma ON usuario.usuario_id = turma.usuario_id
                             WHERE usuario.usuario_cargo = 'Professor'
                             GROUP BY usuario.usuario_id, usuario.usuario_matricula, usuario.usuario_nome");

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
                        <td>{$professor['usuario_matricula']}</td>
                        <td>{$professor['usuario_nome']}</td>
                        <td>{$professor['turmas']}</td>
                        <td><button id='excluir' class='button' onclick='confirmarExclusao({$professor['usuario_id']})'>Excluir</button></td>
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