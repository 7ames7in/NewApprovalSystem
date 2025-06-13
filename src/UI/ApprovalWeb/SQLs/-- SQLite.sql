-- SQLite
Update ApprovalRequests SET ActionStatus = 'Approved', ApplicantPosition = "Manager", ApplicantDepartment = "Sales"
--WHERE ActionStatus = 'Pending' AND Sequence = 1
where ApplicantPosition != ''

WHERE ActionStatus = 'Pending' AND Sequence = 1



select * from Users