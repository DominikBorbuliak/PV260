## 1. User Registration

As a user, I want to create an account so that I can access the system.

### Acceptance Criteria:

- Users can register using an email and password.
- Passwords are stored securely (hashed).
- Registration errors (e.g., invalid email, weak password) are shown to the user.
- Confirmation message is displayed upon successful registration.

### Out of Scope:

- OAuth or third-party authentication.
- Email verification.

### Estimation:

**8** hours

---

## 2. User Login

As a user, I want to log in so that I can access my reports securely.

### Acceptance Criteria:

- Users can log in with an email and password.
- System validates credentials and returns errors if incorrect.
- A session or token is created upon successful login.
- Users are redirected to the dashboard after logging in.

### Out of Scope:

- Multi-factor authentication (MFA).

### Estimation:

**8** hours

---

## 3. User Logout

As a user, I want to log out so that I can securely end my session.

### Acceptance Criteria:

- Users can log out from their account.
- Session or token is invalidated after logout.
- Users are redirected to the login page.

### Out of Scope:

- Auto-logout due to inactivity.

### Estimation:

**1** hour

---

## 4. Display Report Diff

As a user, I want to view a diff report so that I can analyze changes easily.

### Acceptance Criteria:

- The diff report is displayed in a structured table format.
- Increases and decreases are visually differentiated (e.g., color coding).
- The report loads efficiently without noticeable delays.

### Out of Scope:

- Graphs or charts visualizing changes.

### Estimation:

**8** hours

---

## 5. Responsive and Accessible UI

As a user, I want the diff report to be easy to use on all screen sizes and accessible for all users.

### Acceptance Criteria:

- The UI adapts to different screen sizes (mobile, tablet, desktop).
- The interface meets accessibility standards (e.g., proper labels, contrast).

### Out of Scope:

- Custom themes or high-contrast mode.

### Estimation:

**3** hours

---

## 6. Generate New Diff Report

As an admin, I want to click a button to generate the latest diff report so that I can automate client updates.

### Acceptance Criteria:

- Clicking the button triggers the CSV download and diff processing.
- Processing time is reasonable.
- Success message is shown if the report completes successfully.
- Error message is displayed if the process fails.
- Error message provides details (e.g., missing data, API failure).

### Estimation:

**8** hours

---

## 7. Notify Clients About New Diff Report

As an admin, I want to send automatic emails whenever I generate new diff report.

### Acceptance Criteria:

- Once completed, an email is sent to every subscribed user.
- Email contains a message notifying user that a new diff report was generated, not the diff report itself.
- Clients are notified in a reasonable time.

### Estimation:

**6** hours

---

## 8. Restrict Report Generation to Admins

As an admin, I want report generation restricted to admins so that only authorized users can generate reports.

### Acceptance Criteria:

- Non-admin users cannot see or access the report generation button.
- Backend validation ensures only admins can trigger report generation.
- Unauthorized users receive an error message if they try to bypass restrictions.

### Out of Scope:

- Granular role-based permissions (not just admin flag).

### Estimation:

**2** hours

---

## 9. Search for Closest Past Report by Date

As an admin, I want to enter a date and retrieve the closest available past report so that I can review previous data.

### Acceptance Criteria:

- A date input field allows selecting a specific date.
- The system fetches the closest available report from the database.
- The retrieved report is displayed in the UI.

## Estimation:

**3** hours

---

## 10. Download Past Report

As an admin, I want to download a past report so that I can analyze it offline.

### Acceptance Criteria:

- A download button is available next to searched report.
- Clicking the button downloads the report to the user’s device.

### Estimation:

**3** hours

---

## 11. Subscribe to Mailing List

As a user, I want to subscribe to the mailing list so that I receive report updates.

### Acceptance Criteria:

- Users can opt in to receive emails in their profile settings.
- A confirmation message is displayed after subscribing.
- The backend updates the user's preference in the database.

### Estimation:

**1** hour

---

## 12. Unsubscribe from Mailing List

As a user, I want to unsubscribe from the mailing list so that I stop receiving report updates.

### Acceptance Criteria:

- Users can opt out in their profile settings.
- A confirmation message is displayed after unsubscribing.
- The backend updates the user's preference in the database.

### Out of Scope:

- Unsubscribe confirmation emails.
- Unsubscribe via email link.

### Estimation:

**1** hour

---

## 13. Persistently Store Emails and Diff Reports

As an admin, I want to be able to persistently store emais of subscribed users and generated diff reports.

### Acceptance Criteria:

- Every successfully generated diff report will be stored in a persistent storage.
- Every email of a subscribed user is stored in a persistent storage.
- Emails and diff reports share the persistent storage.

### Estimation:

**4** hours
