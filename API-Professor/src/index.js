const express = require('express')
const app = express()
app.use(express.json())

/**
 * name - string
 * employeeRegistration - string
 * password - string
 * cpf - string
 * email - string
 * birthDate - string
 * cellPhone - string
 * classes - []
 * createdAt - string
 */
const employees = [];

/**
 * name - string
 * id - string
 * createdAt - string
 */
const classes = [];

function VerifyIfEmployeeAlreadyExists(req, res, next) {
    const {employeeRegistration} = req.body;
    const employeeAlreadyExists = employees.some((employee) => 
        employee.employeeRegistration === employeeRegistration
    );
    if(employeeAlreadyExists) {
        return res.status(400).json({
            error: "Employee already exists!"
        });
    };
    return next();
};

function verifyIfClassAlreadyExists(req, res, next) {
    const {id} = req.body;
    const classAlreadyExists = classes.some((classId) =>
        classId.id === id
    );
    if(classAlreadyExists) {
        return res.status(400).json({
            error: "Class already exists!"
        });
    };
    return next();
};

function nameFormated(name) {
    return name.toUpperCase();
}

function cpfFormated(cpf) {
    return cpf.substring(0, 3)+"."+cpf.substring(3,6)+"."+cpf.substring(6, 9)+"-"+cpf.substring(9, 11)
};

function cellFormated(number) {
    const areaCode = number.substring(0, 2);    // First two digits for area code
    const prefix = number.substring(2, 7);      // Next five digits for the prefix
    const suffix = number.substring(7, 11);     // Last four digits for the suffix
    return `(${areaCode}) ${prefix}-${suffix}`;
};

function dateFormated() {
    const newDate = new Date();
    const year = newDate.getFullYear();
    const month = monthFormated(newDate.getMonth() + 1);
    const day = dayFormated(newDate.getDate());
    return `${day}/${month}/${year}`;
};

function monthFormated(month) {
    return month < 10 ? '0' + month : month;
}

function dayFormated(day) {
    return day < 10 ? '0' + day : day;
}

function VerifyIfEmployeeExists(req, res, next) {
    const {employeeRegistration} = req.body;
    const employee = employees.find((employee) =>
        employee.employeeRegistration === employeeRegistration
    );
    if(!employee) {
        return res.status(400).json({
            error: "Employee not found!"
        });
    };
    req.employee = employee;
    return next();
};

function verifyIfClassExists(req, res, next) {
    const {id} = req.body;
    const classId = classes.find((classId) =>
        classId.id === id
    );
    if(!classId) {
        return res.status(400).json({
            error: "Class not found!"
        });
    };
    req.classId = classId;
    return next();
};

function verifyIfClassAlreadyLinkedEmployee(req, res, next) {
    const {id} = req.body;
    const {employee} = req;
    if(employee.class.indexOf(id) === -1) {
        return next();
    } else {
        return res.status(400).json({
            error: "Class already linked!"
        });
    };
};

function verifyLogin(req, res, next) {
    const {cpf, password} = req.headers;
    const searchByCPF = employees.find((searchByCPF) =>
        searchByCPF.cpf === cpfFormated(cpf)
    );
    if(!searchByCPF) {
        return res.status(400).json({
            error: "Employee not found!"
        });
    };
    if(searchByCPF.cpf === cpfFormated(cpf) && searchByCPF.password === password) {
        return next();
    } else {
        return res.status(400).json({
            error: "Invalid login!"
        });
    };
};

function verifyClassLinkedEmployee(req, res, next) {
    const {classId} = req;
    for(const valueEmployee of employees) {
        for(const valueClass of valueEmployee.class) {
            if(valueClass === classId.id) {
                return res.status(400).json({
                    error: "Class linked at employee!"
                });
            };
        };
    };
    return next();
};

app.post("/register/employee", VerifyIfEmployeeAlreadyExists, (req, res) => {
    const {name, employeeRegistration, password, cpf, email, birthDate, cellPhone} = req.body;
    employees.push({
        name: nameFormated(name),
        employeeRegistration,
        cpf: cpfFormated(cpf),
        password,
        email,
        birthDate,
        cellPhone: cellFormated(cellPhone),
        createdAt: dateFormated(),
        class: []
    });
    return res.status(201).send();
});

app.post("/register/class", verifyIfClassAlreadyExists, (req, res) => {
    const {name, id} = req.body;
    classes.push({
        name,
        id,
        createdAt: dateFormated()
    });
    return res.status(201).send();
});

app.patch("/register/employee/class", VerifyIfEmployeeExists, verifyIfClassExists, verifyIfClassAlreadyLinkedEmployee, (req, res) => {
    const {employee} = req;
    const {id} = req.body;
    employee.class.push(id);
    return res.status(201).send();
});

app.get("/employees", (req, res) => {
    return res.status(200).send(employees);
});

app.get("/search/registration", VerifyIfEmployeeExists, (req, res) => {
    const {employee} = req;
    return res.status(200).send(employee);
});

app.get("/search/cpf", (req, res) => {
    const {cpf} = req.body;
    const searchByCPF = employees.find((searchByCPF) =>
        searchByCPF.cpf === cpfFormated(cpf)
    );
    if(!searchByCPF) {
        return res.status(400).json({
            error: "Employee not found!"
        });
    };
    return res.status(200).send(searchByCPF);
});

app.get("/search/name", (req, res) => {
    const {name} = req.body;
    const searchByName = employees.find((searchByName) =>
        searchByName.name === nameFormated(name)
    );
    if(!searchByName) {
        return res.status(400).json({
            error: "Employee not found!"
        });
    };
    return res.status(200).send(searchByName);
});

app.get("/classes", (req, res) => {
    return res.status(200).send(classes);
});

app.get("/search/classes/registration", VerifyIfEmployeeExists, (req, res) => {
    const {employee} = req;
    return res.status(200).send(employee.class);
});

app.get("/search/class", verifyIfClassExists, (req, res) => {
    const {classId} = req;
    return res.status(200).send(classId);
});

app.put("/employee", verifyLogin, (req, res) => {
    const {name, password, email, birthDate, cellPhone} = req.body;
    const {cpf} = req.headers;
    const searchByCPF = employees.find((searchByCPF) =>
        searchByCPF.cpf === cpfFormated(cpf)
    );
    if(!searchByCPF) {
        return res.status(400).json({
            error: "Employee not found!"
        });
    };
    searchByCPF.name = nameFormated(name);
    searchByCPF.password = password;
    searchByCPF.email = email;
    searchByCPF.birthDate = birthDate;
    searchByCPF.cellPhone = cellFormated(cellPhone);
    return res.status(200).send();
});

app.delete("/employee", VerifyIfEmployeeExists, (req, res) => {
    const {employee} = req;
    const index = employees.indexOf(employee);
    employees.splice(index, 1);
    return res.status(200).json(employees);
});

app.delete("/class", verifyIfClassExists, verifyClassLinkedEmployee, (req, res) => {
    const { classId } = req;
    const index = classes.findIndex(cls => cls.id === classId.id);
    if (index !== -1) {
        classes.splice(index, 1);
        return res.status(200).json(classes);
    }
    return res.status(400).json({
        error: "Class not found!"
    });
});


app.listen(3333)