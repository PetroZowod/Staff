create database Staff_db;
use Staff_db;
--	������������ ������������� � ��
CREATE TABLE auth_user
(id INT PRIMARY KEY IDENTITY NOT NULL,
users NVARCHAR(50) NOT NULL,
password NVARCHAR(50) NOT NULL);

INSERT INTO auth_user (users, password) VALUES (N'Administrator',N'admin'),(N'���������� ������ ������',N'12345');
-- �����
CREATE TABLE otdel
(id_otdel INT PRIMARY KEY IDENTITY NOT NULL,
otdel_name NVARCHAR(100) NOT NULL);

INSERT INTO otdel(otdel_name) VALUES (N'����� �����������-������������ ���������'), (N'���������������-������������� �����'),
(N'����� �������� ����������'), (N'������������ ���'), (N'����� �������� ������������'), (N'���������������� ���'),
(N'��������-������������ ���'), (N'���������'), (N'��������-������������ ���');

-- ����
CREATE TABLE post 
(id_post INT PRIMARY KEY IDENTITY NOT NULL,
post_name NVARCHAR(100) NOT NULL,
id_otdel INT NOT NULL);

ALTER TABLE post ADD CONSTRAINT FK_Post_to_Otdel FOREIGN KEY (id_otdel) REFERENCES otdel(id_otdel);

INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������'), N'��������� ����������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������'), N'���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������'), N'��������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'������������ ���'), N'��������� ������������� ����');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'������������ ���'), N'�������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'������������ ���'), N'�������� ��������� B');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'������������ ���'), N'�������� ��������� �');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �����������-������������ ���������'), N'��������� �����������-������������ ���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �����������-������������ ���������'), N'����������� �����������-������������ ���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �����������-������������ ���������'), N'���������� �����������-������������ ���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������������-������������� �����'), N'���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������������-������������� �����'), N'��������� ���������������-�������������� ������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �������� ����������'), N'������� ���������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �������� ����������'), N'����������� �������� ����������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �������� ����������'), N'������� ����������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �������� ������������'), N'�����������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'����� �������� ������������'), N'�������� ���������������� ������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������������� ���'), N'��������� ����');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'���������������� ���'), N'������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'��������-������������ ���'), N'��������� ���');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'��������-������������ ���'), N'�������');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'��������-������������ ���'), N'��������� ���');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'��������-������������ ���'), N'����������� ���������� ���');
INSERT INTO post (id_otdel,post_name) VALUES ((select otdel.id_otdel FROM otdel WHERE otdel.otdel_name LIKE N'��������-������������ ���'), N'�������');

--	����������
CREATE TABLE person 
(id_user INT PRIMARY KEY IDENTITY NOT NULL,	--	ID
l_name NVARCHAR(100) NOT NULL,				--	�������
f_name NVARCHAR(100) NOT NULL,				--	���
s_name NVARCHAR(100),						--	��������
is_working bit NOT NULL DEFAULT 1,			--	���������� ��. �������� ��������� ��� ���.
post_id INT NOT NULL,						--	���������
start_work Date NOT NULL,					--	������ ������
end_work Date NULL,							--	����� (��������� ��������)
fired_date Date NULL);						--	���� ����������

--	������� ��� ���������� ��������� �����������
CREATE TABLE old_person 
(id_user INT PRIMARY KEY IDENTITY NOT NULL,	--	ID
tab_name INT,								--	ID ���������� ������� person
l_name NVARCHAR(100) NOT NULL,				--	�������
f_name NVARCHAR(100) NOT NULL,				--	���
s_name NVARCHAR(100),						--	��������
is_working bit NULL,						--	���������� ��. �������� ��������� ��� ���.
post_id INT NOT NULL,						--	���������
start_work Date NOT NULL,					--	������ ������
end_work Date NULL,							--	����� (��������� ��������)
fired_date Date NULL);						--	���� ����������

ALTER TABLE person ADD CONSTRAINT FK_Person_to_Post FOREIGN KEY (post_id) REFERENCES post(id_post);

INSERT INTO person(l_name, f_name, s_name, post_id, start_work, end_work) VALUES (N'������', N'����', N'��������', 1, (SELECT GETDATE()), DATEADD(YEAR, 2, GETDATE()));
INSERT INTO person(l_name, f_name, s_name, post_id, start_work, end_work) VALUES (N'�������', N'����', N'��������', 1, (SELECT GETDATE()), DATEADD(YEAR, 2, GETDATE()));
INSERT INTO person(l_name, f_name, s_name, post_id, start_work, end_work) VALUES (N'������', N'����', N'��������', 1, (SELECT GETDATE()), DATEADD(YEAR, 2, GETDATE()));
