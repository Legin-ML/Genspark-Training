-- Tables to Design (Normalized to 3NF):

-- 1. **students**
--    * `student_id (PK)`, `name`, `email`, `phone`

-- 2. **courses**
--    * `course_id (PK)`, `course_name`, `category`, `duration_days`

-- 3. **trainers**
--    * `trainer_id (PK)`, `trainer_name`, `expertise`

-- 4. **enrollments**
--    * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`

-- 5. **certificates**
--    * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`

-- 6. **course\_trainers** (Many-to-Many if needed)
--    * `course_id`, `trainer_id`



--Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)

CREATE TABLE students(
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(13) NOT NULL
);

CREATE TABLE courses(
    course_id SERIAL PRIMARY KEY,
    course_name TEXT NOT NULL,
    category TEXT,
    duration_days INT NOT NULL
);

CREATE TABLE trainers(
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise TEXT NOT NULL
);

CREATE TABLE enrollments(
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT,
    course_id INT,
    enroll_date DATE,
    FOREIGN KEY (student_id) REFERENCES students(student_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id)
);

CREATE TABLE certificates(
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT,
    issue_date DATE,
    serial_no VARCHAR(100) UNIQUE,
    FOREIGN KEY (enrollment_id) REFERENCES enrollments(enrollment_id)
);

CREATE TABLE course_trainers(
    course_id INT,
    trainer_id INT,
    PRIMARY KEY (course_id, trainer_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id),
    FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id)
);



-- Insert sample data using `INSERT` statements

INSERT INTO students (name, email, phone) VALUES
('Alice Johnson', 'alice.johnson@example.com', '1234567890'),
('Bob Smith', 'bob.smith@example.com', '2345678901'),
('Charlie Lee', 'charlie.lee@example.com', '3456789012');

INSERT INTO courses (course_name, category, duration_days) VALUES
('Basics of DSA', 'Programming', 30),
('Data Science Basics', 'Data Science', 45),
('MERN stack Course', 'Fullstack', 60);

INSERT INTO trainers (trainer_name, expertise) VALUES
('Dr. Jane Doe', 'DSA, Data Science'),
('Mr. John Doe', 'Web Development, Database');

INSERT INTO course_trainers (course_id, trainer_id) VALUES
(1, 1), 
(2, 1),  
(3, 2);  

INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
(1, 2, '2025-03-01'),
(2, 1, '2025-03-15'),
(1, 3, '2025-03-15'),
(3, 2, '2025-04-10');

INSERT INTO certificates (enrollment_id, issue_date, serial_no) VALUES
(1, '2025-04-14', 'DSD-1001'),
(2, '2025-04-15', 'BD-1001'),
(3, '2025-05-15', 'MSC-1001');


-- Create indexes on `student_id`, `email`, and `course_id`

CREATE INDEX idx_students_student_id ON students(student_id);
CREATE UNIQUE INDEX idx_students_email ON students(email);
CREATE INDEX idx_courses_course_id ON courses(course_id);

--------------------------------------------------------------------------------------------------------------

-- Phase 3: SQL Joins Practice

-- Write queries to:

-- 1. List students and the courses they enrolled in

SELECT s.student_id, s.name, c.course_name FROM students as s
JOIN enrollments as e ON s.student_id = e.student_id 
JOIN courses as c ON c.course_id = e.course_id;

-- 2. Show students who received certificates with trainer names

SELECT s.name, t.trainer_name FROM certificates as c
JOIN enrollments as e ON c.enrollment_id = e.enrollment_id
JOIN course_trainers as ct ON e.course_id = ct.course_id
JOIN students as s ON e.student_id = s.student_id
JOIN trainers as t ON ct.trainer_id = t.trainer_id;

-- 3. Count number of students per course

SELECT COUNT(*), c.course_name FROM enrollments as e LEFT JOIN courses as c ON e.course_id = c.course_id GROUP BY c.course_name;

-------------------------------------------------------------------------------------------------------------------------------------------

-- Phase 4: Functions & Stored Procedures

-- Function:

-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.

CREATE OR REPLACE FUNCTION fn_get_certified_students(
	v_course_id INT
)
RETURNS TABLE (
	student_id INT,
	name VARCHAR(100)
) AS $$
BEGIN
	RETURN QUERY
	SELECT s.student_id, s.name
	FROM certificates as c
	JOIN enrollments as e ON c.enrollment_id = e.enrollment_id
	JOIN students as s ON s.student_id = e.student_id
	WHERE e.course_id = v_course_id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM fn_get_certified_students(1);

-- Stored Procedure:

-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).

CREATE OR REPLACE PROCEDURE sp_enroll_student(
	IN p_student_id INT,
	IN p_course_id INT,
	IN p_completed BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
	v_enrollment_id INT;
BEGIN

	INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
	(p_student_id, p_course_id, CURRENT_DATE)
	RETURNING enrollment_id INTO v_enrollment_id;

	
	IF p_completed THEN
		INSERT INTO certificates(enrollment_id, issue_date, serial_no) VALUES
		(v_enrollment_id, CURRENT_DATE, 'CSERIAL-' || v_enrollment_id || '-GI');
	END IF;
		
	RAISE NOTICE 'Student enrolled with enrollment ID: %, Certificate issued: %', v_enrollment_id, p_completed;
END;
$$;

CALL sp_enroll_student(2, 3, TRUE);

SELECT * FROM certificates;

-----------------------------------------------------------------------------------------

-- Phase 5: Cursor

-- Use a cursor to:

-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

CREATE OR REPLACE PROCEDURE sp_list_uncertified_students(p_course_id INT)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;

    cur CURSOR FOR
        SELECT s.name, s.email
        FROM students s
        JOIN enrollments e ON s.student_id = e.student_id
        WHERE e.course_id = p_course_id
          AND NOT EXISTS (
              SELECT 1
              FROM certificates c
              WHERE c.enrollment_id = e.enrollment_id
          );
BEGIN
    OPEN cur;

    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;

        RAISE NOTICE 'Name: %, Email: %', rec.name, rec.email;
    END LOOP;

    CLOSE cur;
END;
$$;

CALL sp_list_uncertified_students(2);



-----------------------------------------------------------------------------------------

-- Phase 6: Security & Roles

-- 1. Create a `readonly_user` role:

--    * Can run `SELECT` on `students`, `courses`, and `certificates`
--    * Cannot `INSERT`, `UPDATE`, or `DELETE`

CREATE ROLE readonly_user NOINHERIT;
GRANT SELECT ON students, courses, certificates TO readonly_user;

ALTER ROLE readonly_user WITH LOGIN PASSWORD 'readonly_password';


-- 2. Create a `data_entry_user` role:

--    * Can `INSERT` into `students`, `enrollments`
--    * Cannot modify certificates directly

CREATE ROLE data_entry_user NOINHERIT;
GRANT INSERT ON students, enrollments TO data_entry_user;
REVOKE INSERT, UPDATE ON certificates FROM data_entry_user;  -- This is already enforced by NOINHERIT

ALTER ROLE data_entry_user WITH LOGIN PASSWORD 'data_entry_user_password';

-------------------------------------------------------------------------


-- Phase 7: Transactions & Atomicity

-- Write a transaction block that:

-- * Enrolls a student
-- * Issues a certificate
-- * Fails if certificate generation fails (rollback)

-- ```sql
-- BEGIN;
-- -- insert into enrollments
-- -- insert into certificates
-- -- COMMIT or ROLLBACK on error
-- ```


CREATE OR REPLACE PROCEDURE sp_enroll_and_certify(
    p_student_id INT,
    p_course_id INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_enrollment_id INT;
BEGIN

    BEGIN
	
        INSERT INTO enrollments (student_id, course_id, enroll_date)
        VALUES (p_student_id, p_course_id, CURRENT_DATE)
        RETURNING enrollment_id INTO v_enrollment_id;

        INSERT INTO certificates (enrollment_id, issue_date, serial_no)
        VALUES (
            v_enrollment_id,
            CURRENT_DATE,
            'CSERIAL-' || v_enrollment_id || '-GI'
        );


    EXCEPTION WHEN OTHERS THEN
        RAISE NOTICE 'Error occurred, rolling back transaction.';
        RAISE;
    END;
END;
$$;

CALL sp_enroll_and_certify(1, 6);

---






