using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace AddressBook
{
    // Класс ContactManager отвечает за управление контактами
    public class ContactManager
    {
        // Список контактов
        private List<Contact> contacts = new List<Contact>();

        // Метод для добавления нового контакта
        public void AddContact(Contact contact)
        {
            // Проверка наличия контакта с таким же номером телефона
            if (contacts.Any(c => c.PhoneNumber == contact.PhoneNumber))
            {
                Console.WriteLine("A contact with such a phone number already exists.");
            }
            else
            {
                // Добавление контакта в список и вывод сообщения об успешном добавлении
                contacts.Add(contact);
                Console.WriteLine("The contact has been added successfully.");
            }
        }

        // Метод для удаления контакта по идентификатору (имени, фамилии или номеру телефона)
        public void DeleteContact(string identifier)
        {
            Contact contact;

            // Попытка преобразовать идентификатор в число (проверка на номер телефона)
            if (int.TryParse(identifier, out _))
            {
                contact = contacts.FirstOrDefault(c => c.PhoneNumber == identifier);
            }
            else
            {
                // Поиск контакта по имени или фамилии
                contact = contacts.FirstOrDefault(c => c.FirstName == identifier || c.LastName == identifier);
            }

            // Если контакт найден, он удаляется из списка
            if (contact != null)
            {
                contacts.Remove(contact);
                Console.WriteLine("The contact was successfully deleted.");
            }
            else
            {
                Console.WriteLine("The contact was not found.");
            }
        }

        // Метод для редактирования контакта по номеру телефона
        public void EditContact(string phoneNumber, Contact newContactData)
        {
            Contact contact = contacts.FirstOrDefault(c => c.PhoneNumber == phoneNumber);

            // Если контакт найден, происходит редактирование
            if (contact != null)
            {
                contact.FirstName = newContactData.FirstName;
                contact.LastName = newContactData.LastName;
                contact.PhoneNumber = newContactData.PhoneNumber;
                contact.Email = newContactData.Email;
                contact.Address = newContactData.Address;

                Console.WriteLine("The contact has been successfully edited.");
            }
            else
            {
                Console.WriteLine("The contact was not found.");
            }
        }

        // Метод для отображения всех контактов
        public void DisplayAllContacts()
        {
            // Проверка наличия контактов для отображения
            if (contacts.Count > 0)
            {
                Console.WriteLine("Contact List:");
                // Вывод контактов в алфавитном порядке
                foreach (var contact in contacts.OrderBy(c => c.FirstName))
                {
                    Console.WriteLine($"FirstName: {contact.FirstName}, LastName: {contact.LastName},Phone Number: {contact.PhoneNumber}, Email: {contact.Email}, Address: {contact.Address}");
                }
            }
            else
            {
                Console.WriteLine("There are no contacts to display.");
            }
        }

        // Метод для поиска контакта по идентификатору (имени, фамилии или номеру телефона)
        public Contact FindContact(string identifier)
        {
            return contacts.FirstOrDefault(c => c.FirstName == identifier || c.LastName == identifier || c.PhoneNumber == identifier);
        }

        // Метод для сохранения контактов в файл
        public void SaveContactsToFile(string filePath)
        {
            // Сериализация списка контактов в формат JSON и сохранение в файл
            string json = JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Contacts have been successfully saved to a file.");
        }

        // Метод для загрузки контактов из файла
        public void LoadContactsFromFile(string filePath)
        {
            // Проверка наличия файла перед загрузкой
            if (File.Exists(filePath))
            {
                // Чтение JSON из файла, десериализация в список контактов
                string json = File.ReadAllText(filePath);
                contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                Console.WriteLine("Contacts have been successfully uploaded from the file.");
            }
            else
            {
                Console.WriteLine("The file was not found. The download is not possible.");
            }
        }

        // Метод для отправки электронного письма контакту
        public void SendEmail(Contact contact, string subject, string body)
        {
            // Проверка наличия адреса электронной почты у контакта
            if (!string.IsNullOrEmpty(contact.Email))
            {
                try
                {
                    // Настройка и отправка электронного письма через SMTP
                    using (var client = new SmtpClient("smtp.example.com"))
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("your_email@example.com", "your_password");
                        client.EnableSsl = true;

                        var message = new MailMessage("your_email@example.com", contact.Email, subject, body);
                        client.Send(message);

                        Console.WriteLine($"The email was successfully sent to {contact.Email}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when sending an email: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("The contact does not have an email address.");
            }
        }
    }
}
