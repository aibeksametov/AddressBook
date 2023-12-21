using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;//Пространство имен System.Net было добавлено, чтобы использовать классы MailMessage и SmtpClient для отправки электронных писем
using System.Net.Mail;

namespace AddressBook
{
    public class Program
    {
        static void Main()
        {
            ContactManager contactManager = new ContactManager();

            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add contact");
                Console.WriteLine("2. Delete contact");
                Console.WriteLine("3. Edit contact");
                Console.WriteLine("4. Search");
                Console.WriteLine("5. Get all contacts");
                Console.WriteLine("6. Save to file");
                Console.WriteLine("7. Load contacts");
                Console.WriteLine("8. Sent Email");
                Console.WriteLine("9. Exit");
                //Каждая операция выполняется в бесконечном цикле (while (true)) с выходом из программы при выборе пользователя "9".
                Console.Write("Select an action (1-9): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddContact(contactManager);
                        break;
                    case "2":
                        DeleteContact(contactManager);
                        break;
                    case "3":
                        EditContact(contactManager);
                        break;
                    case "4":
                        SearchContact(contactManager);
                        break;
                    case "5":
                        contactManager.DisplayAllContacts();
                        break;
                    case "6":
                        SaveContactsToFile(contactManager);
                        break;
                    case "7":
                        LoadContactsFromFile(contactManager);
                        break;
                    case "8":
                        SendEmail(contactManager);
                        break;
                    case "9":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong choice. Please select from 1 to 9.");
                        break;
                }
                Console.ReadLine();
            }

        }

        static void AddContact(ContactManager contactManager)
            //AddContact: Запрашивает у пользователя информацию о новом контакте и добавляет его в менеджер контактов.
        {
            Console.Write("FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Address: ");
            string address = Console.ReadLine();

            Contact newContact = new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address
            };

            contactManager.AddContact(newContact);
        }

        static void DeleteContact(ContactManager contactManager)
            //DeleteContact: Запрашивает у пользователя идентификатор контакта (имя, фамилию или номер телефона) и удаляет соответствующий контакт.
        {
            Console.Write("Enter the first name, last name, or phone number of the contact to delete: ");
            string identifier = Console.ReadLine();

            contactManager.DeleteContact(identifier);
        }

        static void EditContact(ContactManager contactManager)
            //EditContact: Запрашивает у пользователя номер телефона для редактирования существующего контакта, а затем предоставляет возможность ввести новые данные для контакта.
        {
            Console.Write("Enter the phone number of the contact to edit: ");
            string phoneNumber = Console.ReadLine();

            Contact existingContact = contactManager.FindContact(phoneNumber);

            if (existingContact != null)
            {
                Console.WriteLine($"Editing a contact with a phone number {phoneNumber}.");
                Console.Write("New FirstName: ");
                string newFirstName = Console.ReadLine();

                Console.Write("New LastName: ");
                string newLastName = Console.ReadLine();

                Console.Write("New Phone Number: ");
                string newPhoneNumber = Console.ReadLine();

                Console.Write("New Email: ");
                string newEmail = Console.ReadLine();

                Console.Write("New Address: ");
                string newAddress = Console.ReadLine();

                Contact newContactData = new Contact
                {
                    FirstName = newFirstName,
                    LastName = newLastName,
                    PhoneNumber = newPhoneNumber,
                    Email = newEmail,
                    Address = newAddress
                };

                contactManager.EditContact(phoneNumber, newContactData);
            }
            else
            {
                Console.WriteLine("The contact was not found.");
            }
        }

        static void SearchContact(ContactManager contactManager)
        {//SearchContact: Запрашивает у пользователя идентификатор контакта и выводит информацию о контакте, если он найден.
            Console.Write("Enter your first name, last name, or phone number to search for a contact: ");
            string identifier = Console.ReadLine();

            Contact foundContact = contactManager.FindContact(identifier);

            if (foundContact != null)
            {
                Console.WriteLine($"Contact found: FirstName: {foundContact.FirstName}, LastNaem: {foundContact.LastName}, PhoneNumber: {foundContact.PhoneNumber}, Email: {foundContact.Email}, Address: {foundContact.Address}");
            }
            else
            {
                Console.WriteLine("Contact was not found.");
            }
        }

        static void SaveContactsToFile(ContactManager contactManager)
        {//SaveContactsToFile и LoadContactsFromFile: Спрашивает пользователя о пути к файлу и сохраняет/загружает контакты в/из файла
            Console.Write("Enter the path to the file to save the contacts: ");
            string filePath = Console.ReadLine();
            contactManager.SaveContactsToFile(filePath);
        }

        static void LoadContactsFromFile(ContactManager contactManager)
        {
            Console.Write("Enter the file path for uploading contacts: ");
            string filePath = Console.ReadLine();
            contactManager.LoadContactsFromFile(filePath);
        }

        static void SendEmail(ContactManager contactManager)
        {//SendEmail: Запрашивает у пользователя номер телефона контакта, создает электронное письмо и отправляет его.
            Console.Write("Enter the phone number of the contact you want to send an email to: ");
            string phoneNumber = Console.ReadLine();

            Contact contact = contactManager.FindContact(phoneNumber);

            if (contact != null)
            {
                Console.Write("The subject of the letter: ");
                string subject = Console.ReadLine();

                Console.Write("The text of the letter: ");
                string body = Console.ReadLine();

                contactManager.SendEmail(contact, subject, body);
            }
            else
            {
                Console.WriteLine("The contact was not found.");
            }

        }

    }
}
