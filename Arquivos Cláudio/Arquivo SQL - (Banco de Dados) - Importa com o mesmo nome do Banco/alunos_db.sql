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
  `id_Aluno` int(11) NOT NULL,
  `matricula` varchar(20) NOT NULL,
  `Nome` varchar(50) NOT NULL,
  `senha` varchar(11) NOT NULL,
  `genero` varchar(50) DEFAULT NULL,
  `id_turma` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `aluno`
--

INSERT INTO `aluno` (`id_Aluno`, `matricula`, `Nome`, `senha`, `genero`, `id_turma`) VALUES
(1, '11111', 'Lucas', '111', 'Masculino', 1),
(2, '22222', 'Kauã', '222', 'Masculino', 3),
(3, '33333', 'Adrielle', '333', 'Feminino', 4),
(4, '66669', 'Alexandre dos reis gomes correa ', '11122233344', 'Feminino', 1),
(5, '111111', 'pedrinho', 'p123', 'Masculino', 1);

-- --------------------------------------------------------

--
-- Estrutura para tabela `estatisticas`
--

CREATE TABLE `estatisticas` (
  `id_estati` int(11) NOT NULL,
  `id_jogador` int(11) DEFAULT NULL,
  `total_jogado` int(11) DEFAULT NULL,
  `acertos` int(11) DEFAULT NULL,
  `erros` int(11) DEFAULT NULL,
  `inimigos_derrotados` int(11) DEFAULT NULL,
  `moedas_acumuladas` int(11) DEFAULT 0,
  `niveis_desbloqueados` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `progresso`
--

CREATE TABLE `progresso` (
  `id` int(11) NOT NULL,
  `aluno_id` int(11) NOT NULL,
  `topico` varchar(20) NOT NULL,
  `nivel` int(11) NOT NULL,
  `concluido` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `topico`
--

CREATE TABLE `topico` (
  `Id_topico` int(11) NOT NULL,
  `Nome_topico` varchar(15) NOT NULL,
  `Num_Min_topico` int(1) NOT NULL,
  `Num_Max_topico` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `topico`
--

INSERT INTO `topico` (`Id_topico`, `Nome_topico`, `Num_Min_topico`, `Num_Max_topico`) VALUES
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
  `id_turma` int(11) NOT NULL,
  `nome` varchar(50) NOT NULL,
  `id_prof` int(11) DEFAULT NULL,
  `serie` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `turma`
--

INSERT INTO `turma` (`id_turma`, `nome`, `id_prof`, `serie`) VALUES
(1, 'Turma_A', 3, '1 serie'),
(3, 'Turma_B', 3, '2 serie'),
(4, 'Turma_C', 4, '3 serie');

-- --------------------------------------------------------

--
-- Estrutura para tabela `turma_topico`
--

CREATE TABLE `turma_topico` (
  `Id_tur_topi` int(11) NOT NULL,
  `Id_turma` int(11) DEFAULT NULL,
  `Id_topico` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `turma_topico`
--

INSERT INTO `turma_topico` (`Id_tur_topi`, `Id_turma`, `Id_topico`) VALUES
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
  `id_usuario` int(11) NOT NULL,
  `matricula` varchar(20) NOT NULL,
  `Nome` varchar(50) NOT NULL,
  `senha` varchar(11) NOT NULL,
  `cargo_usuario` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuario`
--

INSERT INTO `usuario` (`id_usuario`, `matricula`, `Nome`, `senha`, `cargo_usuario`) VALUES
(1, '123', 'ADM', '123', 'adm'),
(3, '54321', 'Pedro Villa', '321', 'Professor'),
(4, '44444', 'Yago gomes', '580', 'Professor'),
(5, '6969', 'Yago Lindo', 'pedro', 'Professor'),
(6, '123', 'João Lagôas', '123', 'Professor'),
(7, '2368819', 'Cláudio Passos', '2368819', 'Professor');

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `aluno`
--
ALTER TABLE `aluno`
  ADD PRIMARY KEY (`id_Aluno`),
  ADD KEY `id_turma` (`id_turma`);

--
-- Índices de tabela `estatisticas`
--
ALTER TABLE `estatisticas`
  ADD PRIMARY KEY (`id_estati`),
  ADD UNIQUE KEY `id_jogador_2` (`id_jogador`),
  ADD UNIQUE KEY `unique_jogador` (`id_jogador`),
  ADD KEY `id_jogador` (`id_jogador`);

--
-- Índices de tabela `progresso`
--
ALTER TABLE `progresso`
  ADD PRIMARY KEY (`id`),
  ADD KEY `aluno_id` (`aluno_id`);

--
-- Índices de tabela `topico`
--
ALTER TABLE `topico`
  ADD PRIMARY KEY (`Id_topico`);

--
-- Índices de tabela `turma`
--
ALTER TABLE `turma`
  ADD PRIMARY KEY (`id_turma`),
  ADD KEY `id_prof` (`id_prof`);

--
-- Índices de tabela `turma_topico`
--
ALTER TABLE `turma_topico`
  ADD PRIMARY KEY (`Id_tur_topi`),
  ADD KEY `Id_turma` (`Id_turma`),
  ADD KEY `Id_topico` (`Id_topico`);

--
-- Índices de tabela `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id_usuario`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `aluno`
--
ALTER TABLE `aluno`
  MODIFY `id_Aluno` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de tabela `estatisticas`
--
ALTER TABLE `estatisticas`
  MODIFY `id_estati` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `progresso`
--
ALTER TABLE `progresso`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de tabela `topico`
--
ALTER TABLE `topico`
  MODIFY `Id_topico` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de tabela `turma`
--
ALTER TABLE `turma`
  MODIFY `id_turma` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de tabela `turma_topico`
--
ALTER TABLE `turma_topico`
  MODIFY `Id_tur_topi` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de tabela `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `aluno`
--
ALTER TABLE `aluno`
  ADD CONSTRAINT `aluno_ibfk_1` FOREIGN KEY (`id_turma`) REFERENCES `turma` (`id_turma`) ON DELETE SET NULL;

--
-- Restrições para tabelas `estatisticas`
--
ALTER TABLE `estatisticas`
  ADD CONSTRAINT `estatisticas_ibfk_1` FOREIGN KEY (`id_jogador`) REFERENCES `aluno` (`id_Aluno`) ON DELETE CASCADE;

--
-- Restrições para tabelas `progresso`
--
ALTER TABLE `progresso`
  ADD CONSTRAINT `progresso_ibfk_1` FOREIGN KEY (`aluno_id`) REFERENCES `aluno` (`id_Aluno`);

--
-- Restrições para tabelas `turma`
--
ALTER TABLE `turma`
  ADD CONSTRAINT `turma_ibfk_1` FOREIGN KEY (`id_prof`) REFERENCES `usuario` (`id_usuario`) ON DELETE SET NULL;

--
-- Restrições para tabelas `turma_topico`
--
ALTER TABLE `turma_topico`
  ADD CONSTRAINT `turma_topico_ibfk_1` FOREIGN KEY (`Id_turma`) REFERENCES `turma` (`id_turma`) ON DELETE CASCADE,
  ADD CONSTRAINT `turma_topico_ibfk_2` FOREIGN KEY (`Id_topico`) REFERENCES `topico` (`Id_topico`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
