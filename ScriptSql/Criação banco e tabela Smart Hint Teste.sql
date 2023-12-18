CREATE DATABASE `smart_hint`

CREATE TABLE `smart_hint`.cliente (
	nome_cliente varchar(150) NOT NULL COMMENT 'Nome do Cliente/Razão Social',
	email varchar(150) NOT NULL COMMENT ': E-mail do Cliente',
	telefone varchar(11) NOT NULL COMMENT 'Telefone do Cliente',
	data_cadastro TIMESTAMP NULL COMMENT 'Data de Cadastro',
	tipo_pessoa varchar(1) NOT NULL COMMENT 'F - Física, J - Jurídica',
	cpf_cnpj varchar(14) NOT NULL COMMENT 'CPF/CNPJ Cliente',
	inscricao_estadual varchar(12) NULL COMMENT ' Inscrição Estadual',
	isento bool NULL COMMENT 'Isento',
	genero varchar(9) NOT NULL COMMENT 'Gênero',
	data_nascimento timestamp NOT NULL COMMENT 'Data de nascimento do Cliente',
	id varchar(36) default (UUID()) NOT NULL,
	CONSTRAINT cpf_cnpj_uk UNIQUE KEY (cpf_cnpj),
	CONSTRAINT cliente_pk PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `smart_hint`.usuario (
	id varchar(36) NOT NULL COMMENT 'Ligação com o cliente da tabela cliente',
	login_email varchar(150) NOT NULL COMMENT 'E-Mail do Cliente para login',
	senha varchar(100) NOT NULL COMMENT 'Senha Criptografada ',
	situacao_cliente bool DEFAULT false NULL COMMENT 'Situação do Cliente',
	CONSTRAINT usuario_pk PRIMARY KEY (id),
	CONSTRAINT login_email_uk UNIQUE KEY (login_email),
	CONSTRAINT usuario_FK FOREIGN KEY (id) REFERENCES `smart_hint`.cliente(id) ON DELETE CASCADE
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci;

