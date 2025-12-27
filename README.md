# Apps built into this suite (planned apps) #

- An agile work board for tasks (in progress)-(currently engaged)
  - [X] Adding Comments
  - [ ] Adding username to comments per ticket comment
  - [ ] Adding images to ticket comments
- Simple Document Entry Form (CRUD) (backlog)
  - [ ] Adding search ability for common key words
  - [ ] Adding ability to add images to documents
- Client Management System (backlog)
- Invoice Creation (backlog)
- Knowledge bank for in bound calling (backlog)


## Technology Used ##

- .NET 6
- ASP.NET
- Blazor
- SQLite db

## App walkthrough (user flow) ##

1) Sign in
- The app uses ASP.NET Identity for authentication and user management.

2) Land on the ticket board
- The Tickets page is the primary workspace. It shows active vs. completed tickets, plus sorting by severity, due date, assigned user, and assigned company.
- You can open ticket details, edit a ticket, or delete it from the board.

3) Create a new ticket
- Use the “Create New Ticket” flow to add a task with title, description, severity, due date, and assignments.
- Newly created tickets appear on the active list.

4) Work a ticket
- Open ticket details to view the full description, metadata, and status.
- Add tech notes to capture work progress and context.
- Add subtasks to break down work and mark them complete as needed.
- Upload attachments to keep supporting files with the ticket.
- Update the ticket to change severity, due date, assignment, completion state, etc.

5) Review history
- Each update writes a change log entry so you can see what changed over time.

6) Manage companies (supporting data)
- Use the Companies area to create and edit customers/clients.
- Tickets can be assigned to companies for reporting and sorting.

7) Manage users and roles
- Use the Users and Roles pages to manage accounts and permissions.
- Users can edit their own profile details.

## Deep dive: Tickets module ##

The Tickets module is the most complete feature set and acts as the core of the app.

- Ticket list and sorting
  - The Tickets page loads all tickets, splits them by completion status, and allows sorting by key fields.
  - Navigation supports opening details, editing, or deleting a ticket.

- Ticket creation and editing
  - Create Ticket is a form-based flow for entering title, description, severity, due date, and assignments.
  - Edit Ticket updates the same core fields and records changes in a change log.

- Ticket details workspace
  - The details view is the primary “work surface” for a ticket.
  - Tech notes are added and displayed with the ticket.
  - Subtasks can be added per ticket and toggled complete.
  - Attachments can be uploaded and removed from the ticket.

- Change tracking
  - Updates generate change log entries to capture what changed and who changed it.

- Data model highlights
  - A ticket links to a company and an assigned user.
  - Tickets can have many tech notes (via a join entity), subtasks, attachments, and change logs.
