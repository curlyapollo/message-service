# message-service
An ASP.Net Core application for working with the messaging service has been developed.
This program implements the functionality for viewing
information about users, their messages and their addition to the system. The user is characterized by a name and an email address. The email is used as an identifier and is therefore unique (two users cannot have the same e-mail address). A message is characterized by a subject, textual content, a sender user and a recipient.
The program has the following functionality:
1. A mechanism for reading and writing a list of users and messages from the corresponding JSON files (to the corresponding JSON files).
The list of users is ordered (it is sorted lexicographically by mail address (Email) in ascending order). Users have two properties - string UserName and string Email.
Messages have four properties: Subject, Message,
SenderId and ReceiverId.
This one is used by the handlers listed below.
2. A handler which is available via the POST method for initializing (ie, initial filling) the list of users and the list of messages randomly (using Random) has been implemented. Lists are filled using the specified handler.
3. Two handlers available only through the GET method have been implemented:
a) to obtain information about the user by his identifier (Email), considering that when the user is absent, the response code should be HTTP 404 (not found);
b) to get the entire list of users.
4. A GET handler to get a list of messages by sender and recipient IDs has been implemented.
5. GET handlers have been implemented. They allow to get a list of messages:
a) by sender ID (recipient - any);
b) by recipient ID (sender - any).
6. Swagger should be used to get auto-generated documentation for implemented handlers.
7. It is possible to register new users: a handler (POST) adding information about the new user to the system has been implemented.
8. It is possible to send messages: a handler (POST) that adds information about a new message has been created. It is possible to check if the sender and recipient of the message are registered users (that means they exist in the list of users). If at least one of the users is not in the list, an error message is returned (the message itself is not saved).
9. Improved handler for receiving
the list of all users (item 3b), with the help of the functionality support for page-by-page selection - support for the Limit and Offset parameters has been added:
• int Limit – the number of users to be returned (maximum);
• int Offset – serial number of the user, starting from which it is necessary to obtain information (in other words, the number of
users to skip from the beginning of the list).
Negative Offset values   or non-positive Limit values   return HTTP 400 (bad request).
List of users to determine the sequence number
sorted lexicographically by Email (ascending).
