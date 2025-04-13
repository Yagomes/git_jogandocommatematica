-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 03/04/2025 às 20:18
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `alunos_db`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `aluno`
--

CREATE TABLE `aluno` (
  `aluno_id` int(11) NOT NULL,
  `aluno_matricula` varchar(20) NOT NULL,
  `aluno_nome` varchar(50) NOT NULL,
  `aluno_senha` varchar(11) NOT NULL,
  `aluno_genero` varchar(50) DEFAULT NULL,
  `turma_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `aluno`
--

INSERT INTO `aluno` (`aluno_id`, `aluno_matricula`, `aluno_nome`, `aluno_senha`, `aluno_genero`, `turma_id`) VALUES
(1, '11111', 'Lucas', '111', 'Masculino', 1),
(2, '22222', 'Kauã', '222', 'Masculino', 3),
(3, '33333', 'Vitor', '333', 'Masculino', 4),
(4, '44444', 'Ana', '444', 'Feminino', 1),
(5, '55555', 'Pedro', '555', 'Masculino', 1);

-- --------------------------------------------------------

--
-- Estrutura para tabela `estatistica`
--

CREATE TABLE `estatistica` (
  `estatistica_id` int(11) NOT NULL,
  `aluno_id` int(11) DEFAULT NULL,
  `estatistica_total_jogado` int(11) DEFAULT 0,
  `estatistica_acertos` int(11) DEFAULT 0,
  `estatistica_erros` int(11) DEFAULT 0,
  `estatistica_inimigos_derrotados` int(11) DEFAULT 0,
  `estatistica_moedas_acumuladas` int(11) DEFAULT 0,
  `estatistica_niveis_desbloqueados` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `progresso`
--

CREATE TABLE `progresso` (
  `progresso_id` int(11) NOT NULL,
  `aluno_id` int(11) NOT NULL,
  `progresso_topico` varchar(20) NOT NULL,
  `progresso_nivel` int(11) NOT NULL,
  `progresso_concluido` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `topico`
--

CREATE TABLE `topico` (
  `topico_id` int(11) NOT NULL,
  `topico_nome` varchar(15) NOT NULL,
  `topico_num_min` int(11) NOT NULL,
  `topico_num_max` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `topico`
--

INSERT INTO `topico` (`topico_id`, `topico_nome`, `topico_num_min`, `topico_num_max`) VALUES
(1, 'soma', 1, 50),
(2, 'mult', 1, 5),
(3, 'soma', 5, 7),
(4, 'sub', 1, 10),
(5, 'div', 1, 20),
(6, 'soma', -17, -8);

-- --------------------------------------------------------

--
-- Estrutura para tabela `turma`
--

CREATE TABLE `turma` (
  `turma_id` int(11) NOT NULL,
  `turma_nome` varchar(50) NOT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `turma_serie` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `turma`
--

INSERT INTO `turma` (`turma_id`, `turma_nome`, `usuario_id`, `turma_serie`) VALUES
(1, 'Turma_A', 3, '1 serie'),
(3, 'Turma_B', 3, '2 serie'),
(4, 'Turma_C', 4, '3 serie');

-- --------------------------------------------------------

--
-- Estrutura para tabela `turma_topico`
--

CREATE TABLE `turma_topico` (
  `tur_topi_id` int(11) NOT NULL,
  `turma_id` int(11) DEFAULT NULL,
  `topico_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `turma_topico`
--

INSERT INTO `turma_topico` (`tur_topi_id`, `turma_id`, `topico_id`) VALUES
(2, 1, 2),
(3, 3, 1),
(4, 4, 2),
(7, 4, 3),
(8, 3, 3),
(9, 3, 3),
(10, 1, 4),
(11, 1, 5),
(12, 1, 5),
(14, 1, 4);

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuario`
--

CREATE TABLE `usuario` (
  `usuario_id` int(11) NOT NULL,
  `usuario_matricula` varchar(20) NOT NULL,
  `usuario_nome` varchar(50) NOT NULL,
  `usuario_senha` varchar(11) NOT NULL,
  `usuario_cargo` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuario`
-- jogador_id

INSERT INTO `usuario` (`usuario_id`, `usuario_matricula`, `usuario_nome`, `usuario_senha`, `usuario_cargo`) VALUES
(1, '111', 'ADM', '777', 'adm'),
(3, '222', 'Pedro Vila', '888', 'Professor'),
(4, '333', 'Yago Gomes', '999', 'Professor'),
(5, '444', 'Lucas', 'pedro', 'Professor'),
(6, '555', 'João', '999', 'Professor'),
(7, '666', 'Cláudio Passos', '100', 'Professor');

 

--
-- Índices de tabela `aluno`
--
ALTER TABLE `aluno`
  ADD PRIMARY KEY (`aluno_id`),
  ADD KEY `turma_id` (`turma_id`);

--
-- Índices de tabela `estatistica`
--
ALTER TABLE `estatistica`
  ADD PRIMARY KEY (`estatistica_id`),
  ADD UNIQUE KEY `aluno_id_2` (`aluno_id`),
  ADD UNIQUE KEY `unique_aluno` (`aluno_id`),
  ADD KEY `aluno_id` (`aluno_id`);

--
-- Índices de tabela `progresso`
--
ALTER TABLE `progresso`
  ADD PRIMARY KEY (`progresso_id`),
  ADD KEY `aluno_id` (`aluno_id`);

--
-- Índices de tabela `topico`
--
ALTER TABLE `topico`
  ADD PRIMARY KEY (`topico_id`);

--
-- Índices de tabela `turma`
--
ALTER TABLE `turma`
  ADD PRIMARY KEY (`turma_id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- Índices de tabela `turma_topico`
--
ALTER TABLE `turma_topico`
  ADD PRIMARY KEY (`tur_topi_id`),
  ADD KEY `turma_id` (`turma_id`),
  ADD KEY `topico_id` (`topico_id`);

--
-- Índices de tabela `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`usuario_id`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `aluno`
--
ALTER TABLE `aluno`
  MODIFY `aluno_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de tabela `estatistica`
--
ALTER TABLE `estatistica`
  MODIFY `estatistica_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `progresso`
--
ALTER TABLE `progresso`
  MODIFY `progresso_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `topico`
--
ALTER TABLE `topico`
  MODIFY `topico_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `turma`
--
ALTER TABLE `turma`
  MODIFY `turma_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `turma_topico`
--
ALTER TABLE `turma_topico`
  MODIFY `tur_topi_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuario`
--
ALTER TABLE `usuario`
  MODIFY `usuario_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `aluno`
--
ALTER TABLE `aluno`
  ADD CONSTRAINT `aluno_ibfk_1` FOREIGN KEY (`turma_id`) REFERENCES `turma` (`turma_id`) ON DELETE SET NULL;

--
-- Restrições para tabelas `estatistica`
--
ALTER TABLE `estatistica`
  ADD CONSTRAINT `estatistica_ibfk_1` FOREIGN KEY (`aluno_id`) REFERENCES `aluno` (`aluno_id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `progresso`
--
ALTER TABLE `progresso`
  ADD CONSTRAINT `progresso_ibfk_1` FOREIGN KEY (`aluno_id`) REFERENCES `aluno` (`aluno_id`);

--
-- Restrições para tabelas `turma`
--
ALTER TABLE `turma`
  ADD CONSTRAINT `turma_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`usuario_id`) ON DELETE SET NULL;

--
-- Restrições para tabelas `turma_topico`
--
ALTER TABLE `turma_topico`
  ADD CONSTRAINT `turma_topico_ibfk_1` FOREIGN KEY (`turma_id`) REFERENCES `turma` (`turma_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `turma_topico_ibfk_2` FOREIGN KEY (`topico_id`) REFERENCES `topico` (`topico_id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
