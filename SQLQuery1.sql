INSERT INTO VolunteersDb (FullName, Password, Gmail, PhoneNumber, Specialization, Region, City)
VALUES
('David Cohen', '1234', 'david@gmail.com', '0501234567', '����', '����', '�� ����'),
('Sara Levi', 'abcd', 'sara@gmail.com', '0527654321', '����� ���� ������', '����', '����'),
('Moti Azulay', 'qwer', 'moti@gmail.com', '0549876543', '��� �������', '����', '��� ���'),
('Rachel Dahan', '9999', 'rachel@gmail.com', '0534567890', '����������', '�������', '�������');
INSERT INTO UsersDb (FirstName, LastNAme, PhoneNumber, Gmail, Password)
VALUES
('Moshe', 'Cohen', '0501112233', 'moshe@gmail.com', 'pass1'),
('Lea', 'Biton', '0522223344', 'lea@gmail.com', 'pass2'),
('Yossi', 'Mizrahi', '0533334455', 'yossi@gmail.com', 'pass3'),
('Rina', 'Shalom', '0544445566', 'rina@gmail.com', 'pass4');
INSERT INTO CallsDb(LocationX, LocationY, ImageUrl, Description, UrgencyLevel, Status, numVolanteer)
VALUES
(32.0853, 34.7818, 'https://example.com/image1.jpg', '����� ����� ��� ����� �������', 2, '����', 1),
(31.7683, 35.2137, 'https://example.com/image2.jpg', '��� ����� ���� ����� ������', 5, '������', 3),
(32.1093, 34.8555, NULL, '����� ����� ����� �������', 3, '����', 2),
(31.2518, 34.7913, 'https://example.com/image3.jpg', '����� ��� �����', 4, '����', 4);


INSERT INTO VolunteerCallsDb (CallsId, VolunteerId, TreatmentDateTime)
VALUES
(1, 2, '2025-05-12 14:30:00'),
(2, 1, '2025-05-11 09:15:00'),
(3, 3, '2025-05-10 17:45:00'),
(1, 4, '2025-05-12 15:00:00');
