CREATE DATABASE `smart_hint`

CREATE TABLE `cliente` (
  `NomeCliente` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Nome do Cliente/Razão Social',
  `Email` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT ': E-mail do Cliente',
  `Telefone` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Telefone do Cliente',
  `DataCadastro` timestamp NULL DEFAULT NULL COMMENT 'Data de Cadastro',
  `TipoPessoa` varchar(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'F - Física, J - Jurídica',
  `CpfCnpj` varchar(14) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'CPF/CNPJ Cliente',
  `InscricaoEstadual` varchar(12) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL COMMENT ' Inscrição Estadual',
  `Isento` tinyint(1) DEFAULT '0' COMMENT 'Isento',
  `Genero` varchar(9) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Gênero',
  `DataNascimento` timestamp NOT NULL COMMENT 'Data de nascimento do Cliente',
  `Id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'uuid()',
  `Senha` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'Senha ',
  `ClienteBloqueado` tinyint(1) DEFAULT '0' COMMENT 'Situação do Cliente',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



