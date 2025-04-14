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

$id_professor = $_SESSION['usuario_id'];

// Inicializa mensagens de alerta
$alerta = "";
$alerta2 = "";

// Lógica para gerenciar tópicos (adicionar/atualizar/excluir)
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if (isset($_POST['action']) && $_POST['action'] == 'manage_topics') {
        $id_turma = isset($_POST['turma']) ? $_POST['turma'] : '';
        $selected_topics = isset($_POST['topicos']) ? $_POST['topicos'] : [];
        $existing_topics = isset($_POST['existing_topics']) ? explode(',', $_POST['existing_topics']) : [];

        // Verifica se há tópicos com o mesmo nome selecionados
        $selected_topic_names = [];
        $duplicate_found = false;

        foreach ($selected_topics as $id_topico) {
            $sql = "SELECT topico_nome FROM topico WHERE topico_id = '$id_topico'";
            $result = $conn->query($sql);

            if ($result->num_rows > 0) {
                $row = $result->fetch_assoc();
                $nome_topico = $row['topico_nome'];

                if (in_array($nome_topico, $selected_topic_names)) {
                    $duplicate_found = true;
                    break;
                } else {
                    $selected_topic_names[] = $nome_topico;
                }
            }
        }

        if ($duplicate_found) {
            $alerta2 = "Não é permitido selecionar dois ou mais tópicos com o mesmo nome.";
        } else {
            if (!empty($id_turma)) {
                // Verifica tópicos desmarcados (para deletar)
                foreach ($existing_topics as $id_topico) {
                    if (!in_array($id_topico, $selected_topics)) {
                        $sql_delete = "DELETE FROM turma_topico WHERE turma_id = '$id_turma' AND topico_id = '$id_topico'";
                        $conn->query($sql_delete);
                    }
                }

                // Verifica tópicos marcados (para adicionar)
                foreach ($selected_topics as $id_topico) {
                    if (!in_array($id_topico, $existing_topics)) {
                        $sql_insert = "INSERT INTO turma_topico (turma_id, topico_id) VALUES ('$id_turma', '$id_topico')";
                        $conn->query($sql_insert);
                    }
                }

                $alerta = "Tópicos da turma atualizados com sucesso!";
            } else {
                $alerta2 = "Por favor, selecione uma turma.";
            }
        }
    }

    // Lógica para excluir um tópico
    if (isset($_POST['action']) && $_POST['action'] == 'delete_topic') {
        $id_topico = isset($_POST['topico_id']) ? $_POST['topico_id'] : '';

        if (!empty($id_topico)) {
            // Excluir o tópico da tabela 'topico'
            $sql_delete = "DELETE FROM topico WHERE topico_id = '$id_topico'";
            if ($conn->query($sql_delete) === TRUE) {
                $alerta = "Tópico excluído com sucesso!";
            } else {
                $alerta2 = "Erro ao excluir o tópico: " . $conn->error;
            }
        } else {
            $alerta2 = "ID do tópico não foi fornecido.";
        }
    }
}

// Busca turmas do professor
$sql = "SELECT * FROM turma WHERE usuario_id = '$id_professor'";
$result = $conn->query($sql);
$turmas = [];

if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $turmas[] = $row;
    }
}

// Busca tópicos existentes
$sql = "SELECT * FROM topico ORDER BY topico_nome ASC";
$result = $conn->query($sql);
$topicos = [];

if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $topicos[] = $row;
    }
}

// Busca tópicos associados à turma
$turma_topics = [];

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST['action']) && $_POST['action'] == 'load_topics') {
    $id_turma = isset($_POST['turma']) ? $_POST['turma'] : '';

    if (!empty($id_turma)) {
        $sql = "SELECT topico.* FROM topico 
                INNER JOIN turma_topico ON topico.topico_id = turma_topico.topico_id 
                WHERE turma_topico.turma_id = '$id_turma'";
        $result = $conn->query($sql);

        if ($result->num_rows > 0) {
            while ($row = $result->fetch_assoc()) {
                $turma_topics[] = $row['topico_id'];
            }
        }
    }
}

$conn->close();
?>

<!DOCTYPE html>
<html>

<head>
    <title>Gerenciar Turma</title>
    <link rel="stylesheet" type="text/css" href="Geren_Turma.css">
    <script>
        function confirmExclusion(id) {
            if (confirm("Tem certeza que deseja excluir este tópico?")) {
                document.getElementById('action').value = 'delete_topic';
                document.getElementById('topico_id').value = id;
                document.getElementById('form').submit();
            }
        }

        function loadTopics() {
            document.getElementById('action').value = 'load_topics';
            document.getElementById('form').submit();
        }
    </script>
</head>

<body>
    <div class="container">
        <h1>Gerenciar Turma</h1>

        <!-- Mensagem de alerta -->
        <?php if (!empty($alerta)): ?>
            <p class="alert"><?php echo $alerta; ?></p>
        <?php endif; ?>
        <?php if (!empty($alerta2)): ?>
            <p class="alert2"><?php echo $alerta2; ?></p>
        <?php endif; ?>

        <!-- Formulário para gerenciar tópicos -->
        <form method="post" action="" id="form">
            <input type="hidden" name="action" id="action">
            <input type="hidden" name="topico_id" id="topico_id">

            <label for="turma">Selecione a Turma:</label>
            <select id="turma" name="turma" onchange="loadTopics()" required>
                <option value="">Selecione</option>
                <?php foreach ($turmas as $turma): ?>
                    <option value="<?php echo $turma['turma_id']; ?>" <?php echo isset($_POST['turma']) && $_POST['turma'] == $turma['turma_id'] ? 'selected' : ''; ?>>
                        <?php echo $turma['turma_nome']; ?>
                    </option>
                <?php endforeach; ?>
            </select><br><br>

            <h3>Escolha os Tópicos Matemáticos:</h3>
            <table class="topic-table">
                <thead>
                    <tr>
                        <th>Selecionar</th>
                        <th>Operação</th>
                        <th>Mínimo</th>
                        <th>Máximo</th>
                        <th>Excluir</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($topicos as $topico): ?>
                    <tr>
                        <td>
                            <input type="checkbox" name="topicos[]" value="<?php echo $topico['topico_id']; ?>" 
                            <?php echo in_array($topico['topico_id'], $turma_topics) ? 'checked' : ''; ?>>
                        </td>
                        <td><?php echo $topico['topico_nome']; ?></td>
                        <td><?php echo $topico['topico_num_min']; ?></td>
                        <td><?php echo $topico['topico_num_max']; ?></td>
                        <td>
                            <button type="button" class="excluir" onclick="confirmExclusion(<?php echo $topico['topico_id']; ?>)">Excluir</button>
                        </td>
                    </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>

            <input type="hidden" id="existing_topics" name="existing_topics"
                value="<?php echo implode(',', $turma_topics); ?>">
                <br>
                
            <button type="submit" onclick="document.getElementById('action').value = 'manage_topics';">Atualizar Tópicos da Turma</button>
        </form>

        <br>
        <a href="../Criar Tópico/Criar_Topico.php"><button>Criar Novo Tópico</button></a>
        <br>
        <br>               
        <a href="../../T_Professor/Tela_Professor.php"><button class="voltar">Voltar</button></a>
    