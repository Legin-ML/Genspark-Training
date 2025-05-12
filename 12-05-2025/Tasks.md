---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
12 May 2025: Transactions and Concurrency

1️⃣ Question:
In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
Will my first two updates persist?

    No, The database will roll back to a state before the transaction started

2️⃣ Question:
Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?

    No, Unless Transaction A is commited, Transaction B cannot read it.

3️⃣ Question:
What will happen if two concurrent transactions both execute:
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
at the same time? Will one overwrite the other?
    
    No, as when a transaction starts, MVCC locks the affected rows. MVCC allows Read & Write, Write & Read, but not Write & Write.
       (This is allowed under SQLServer as Locking is not enforced by default. This is a "Lost Update" scenario)

4️⃣ Question:
If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?

    It will only undo the changes made after the savepoint

5️⃣ Question:
Which isolation level in PostgreSQL prevents phantom reads?

    Serializable

6️⃣ Question:
Can Postgres perform a dirty read (reading uncommitted data from another transaction)?

    No, as postgres does not support reading uncommited data

7️⃣ Question:
If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?

    Yes, the changes are commited immediately.    

8️⃣ Question:
If I do this:

BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- (No COMMIT yet)
And from another session, I run:

SELECT balance FROM accounts WHERE id = 1;
Will the second session see the deducted balance?

    No, the second session will not see the deducted balance
