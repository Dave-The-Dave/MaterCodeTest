//get required elements - buttons
const admit = document.getElementById("AdmitWindow");
const newPatient = document.getElementById("NewPatientWindow");
const patientDetails = document.getElementById("PatientWindow");
const discharge = document.getElementById("DischargeWindow");
const comment = document.getElementById("CommentWindow");
const admitSpan = document.getElementsByClassName("close")[0];
const patientSpan = document.getElementsByClassName("close")[1];
const dischargeSpan = document.getElementsByClassName("close")[2];
const commentSpan = document.getElementsByClassName("close")[3];
const patientDetailsSpan = document.getElementsByClassName("close")[4];
const newBtn = document.getElementById("newBtn");
const existingBtn = document.getElementById("existingBtn");
const saveAdmit =  document.getElementById("confirmAdmit");
const saveNewPatientAdmit =  document.getElementById("confirmNewPatientAdmit");
const confirmDischarge = document.getElementById("confirmDischarge")
const cancelDischarge = document.getElementById("cancelDischarge")
const saveComment = document.getElementById("saveComment")

//data Fields
const bedsInUse = document.getElementById("bedsInUse");
const bedsFree = document.getElementById("bedsFree");
const totalPatientsToday = document.getElementById("TotalPatientsToday");
const patientData = document.getElementById("patientData")

//inputs
const patientURN = document.getElementById("patientURN");
const patientFirstName = document.getElementById("patientFirstName");
const patientLastName = document.getElementById("patientLastName");
const patientDOB = document.getElementById("patientDOB");
const patientIssues = document.getElementById("patientIssues");
const existingURN = document.getElementById("existingURN");
const staffMemberId = document.getElementById("commentStaff");
const commentText = document.getElementById("commentText");

//work around row selector
let selectedBed;


//load data into table
async function loadIntoTable(table) {
    const tableBody = table.querySelector("tbody");

    //get required data
    const response = await fetch("https://localhost:7150/api/PatientAdmition/GetBeds")
    const beds = await response.json();

    console.log(beds);
    //clear any existing content
    tableBody.innerHTML = "";

    //populate rows
    for (const bed of beds) {
        //new row
        const rowElement = document.createElement("tr");

        //define fields
        const bodyElementID = document.createElement("th");
        const bodyElementStatus = document.createElement("td");
        const bodyElementPatient = document.createElement("td");
        const bodyElementDOB = document.createElement("td");
        const bodyElementIssue = document.createElement("td");
        const bodyElementLastComment = document.createElement("td");
        const bodyElementLastUpdate = document.createElement("td");
        const bodyElementNurse = document.createElement("td");
        const bodyElementActions = document.createElement("td");
        
        //define buttons
        const admitBtn = document.createElement('input');
        admitBtn.type = "button";
        admitBtn.className = "admitBtn";
        admitBtn.value = "Admit";
        admitBtn.onclick = function(){
            admit.style.display = "block";
            selectedBed = bed;
        }
        
        const commentBtn = document.createElement('input');
        commentBtn.type = "button";
        commentBtn.className = "commentBtn";
        commentBtn.value = "Add Comment";
        commentBtn.onclick = function(){
            comment.style.display = "block";
            selectedBed = bed;
        }

        const dischargeBtn = document.createElement('input');
        dischargeBtn.type = "button";
        dischargeBtn.className = "dischargeBtn";
        dischargeBtn.value = "Discharge Paitent";
        dischargeBtn.onclick = function(){
            discharge.style.display = "block";
            selectedBed = bed;
        }

        //build table
        bodyElementID.textContent = bed.bedId;
        //if bed is not in use
        if (bed.vaccancy == 0) {
            bodyElementStatus.textContent = "Free";
            bodyElementPatient.textContent = "";
            bodyElementDOB.textContent = "";
            bodyElementIssue.textContent = "";
            bodyElementLastComment.textContent = "";
            bodyElementLastUpdate.textContent = "";
            bodyElementNurse.textContent = "";
            bodyElementActions.appendChild(admitBtn);
        }
        else {
            const admitDate = new Date( bed.patient.comments[bed.patient.comments.length -1].loggedDate)
            bodyElementStatus.textContent = "In Use";
            bodyElementPatient.textContent = bed.patient.firstName + " " + bed.patient.lastName;
            bodyElementDOB.textContent = bed.patient.dob;
            bodyElementIssue.textContent = bed.patient.presentingIssues;
            bodyElementLastComment.textContent = bed.patient.comments[bed.patient.comments.length -1].loggedComment;
            bodyElementLastUpdate.textContent = admitDate.toLocaleString();
            bodyElementNurse.textContent = bed.patient.comments[bed.patient.comments.length -1].staffMember.name;
            bodyElementActions.appendChild(commentBtn);
            bodyElementActions.appendChild(dischargeBtn);

            //if bed is in use, add click functionality to row
            rowElement.onclick = function(event){
                //dont run if users clicked the actions column
                if (event.target.className == "commentBtn" || event.target.className == "dischargeBtn") {
                    return;
                }
                selectedBed = bed;
                    openPatientDetails();               
            }
        }

        //add data to row
        rowElement.appendChild(bodyElementID);
        rowElement.appendChild(bodyElementStatus);
        rowElement.appendChild(bodyElementPatient);
        rowElement.appendChild(bodyElementDOB);
        rowElement.appendChild(bodyElementIssue);
        rowElement.appendChild(bodyElementLastComment);
        rowElement.appendChild(bodyElementLastUpdate);
        rowElement.appendChild(bodyElementNurse);
        rowElement.appendChild(bodyElementActions);
        
        //append to table
        tableBody.appendChild(rowElement);


    }    
    
    //pass bed table into total calculations
    getTotals(beds);
}

//summary data
async function getTotals(beds){
    const totalBedsUsed = beds.filter(x => x.admitionId != null);
    const totalBedsFree = beds.filter(x => x.admitionId == null); //this method will still function if number of beds ever change

    const response = await fetch("https://localhost:7150/api/PatientAdmition/GetAllAdmittions");
    const admitions = await response.json();
    let today = new Date().toISOString().slice(0, 10)

    bedsInUse.innerHTML = `Beds In Use: ${totalBedsUsed.length}`;
    bedsFree.innerHTML = `Beds free: ${totalBedsFree.length}`;
    totalPatientsToday.innerHTML = `Total Patients Today: ${admitions.filter(x => x.date.slice(0,10)== today).length}`;


}

//API Calls
async function admitPatient(bed, patient){
    const body = {
        AdmitionId:0,
        BedId: bed.bedId,
        patientURN: patient.patientURN,
        PatientName: patient.firstName + " " + patient.lastName
    }

    fetch("https://localhost:7150/api/PatientAdmition/CreateEdit",{
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            "content-type": "application/json"
        }
    })
    .then(data => data.json())
    .then(response => console.log(response))
    .catch(error => {
        console.log(error)
    });
}

async function addPatient(patient){
    fetch("https://localhost:7150/api/Patient/CreateEdit", {
        method: 'POST',
        body: JSON.stringify(patient),
        headers: {
            "content-type": "application/json"
        }
    })
    .then(data => data.json())
    .then(response => console.log(response))
    .catch(error => {
        console.log(error)
    });
}

async function dischargePatient(){
    await fetch("https://localhost:7150/api/PatientAdmition/Discharge?id="+selectedBed.admitionId, {
        method: 'POST',
        headers: {
            "content-type": "application/json"
        }
    })
    .then(data => data.json())
    .then(response => console.log(response))
    .catch(error => {
        console.log(error)
    });
}

async function addComment(comment){
    await fetch("https://localhost:7150/api/Patient/AddComment",{
        method: 'POST',
        body: JSON.stringify(comment),
        headers: {
            "content-type": "application/json"
        }
    })
    .then(data => data.json())
    .then(response => console.log(response))
    .catch(error => {
        console.log(error)
    });
}

async function openPatientDetails() {
    const response = await fetch("https://localhost:7150/api/Patient/GetByURN?URN=" + selectedBed.patientURN)
    const patients = await response.json();
    const patient = patients[0];

    // set up patient details based on results
    const patientDataHtml =
        `<div class="detailRow">
            <div>Name: ${patient.firstName + " " + patient.lastName}</div>
        </div>
        <div class="detailRow">
            <div>URN: ${patient.patientURN}</div>
        </div>
        <div class="detailRow">
            <div>Date of Birth: ${patient.dob}</div>
        </div>
        <div class="detailRow">
            <div>Bed: ${selectedBed.bedId}</div>
        </div>
        <div class="detailRow">
            <div>Presenting Issues: ${patient.presentingIssues}</div>
        </div>
        <h5>Comments: </h5>`;
    patientData.innerHTML= patientDataHtml;

    //load comments table
    const table = document.getElementById("commentTable");
    const tableBody = table.querySelector("#PatientComments");
    for (const comment of patient.comments) {
        //new row
        const rowElement = document.createElement("tr");

        //define fields
        const bodyElementDate = document.createElement("td");
        const bodyElementTime = document.createElement("td");
        const bodyElementNurse = document.createElement("td");
        const bodyElementComment = document.createElement("td");

        //populate
        const commentDate = new Date(comment.loggedDate);
        bodyElementDate.textContent = commentDate.toLocaleDateString();
        bodyElementTime.textContent = commentDate.toLocaleTimeString();
        bodyElementNurse.textContent = comment.staffMember.name;
        bodyElementComment.textContent = comment.loggedComment;

        //add to row
        rowElement.appendChild(bodyElementDate);
        rowElement.appendChild(bodyElementTime);
        rowElement.appendChild(bodyElementNurse);
        rowElement.appendChild(bodyElementComment);

        //add to table
        tableBody.appendChild(rowElement);
    }

    //show patient details
    patientDetails.style.display= "block"
}

//load data on page load
loadIntoTable( document.querySelector("table"));

//set up controls
newBtn.onclick = function(event) {
    admit.style.display = "none";
    newPatient.style.display = "block";
}

saveAdmit.onclick= async function(event){
    
    const response = await fetch("https://localhost:7150/api/Patient/GetByURN?URN=" + existingURN.value)
    .catch(error => {
        console.log(error)
    });
    const selectedPatient = await response.json();

    if (selectedPatient == null) throw Error("URN not found");
    admitPatient(selectedBed, selectedPatient[0]);
    admit.style.display = "none";

    location.reload();
}

saveNewPatientAdmit.onclick = async function(event){
    const newPatient = {
        patientURN: patientURN.value,
        firstName: patientFirstName.value,
        lastName: patientLastName.value,
        dob: patientDOB.value,
        presentingIssues: patientIssues.value
    }
    
    addPatient(newPatient);
    admitPatient(selectedBed,newPatient);
    newPatient.style.display="none";
    location.reload();
}

confirmDischarge.onclick = async function(event){
    await dischargePatient();
    discharge.style.display = "none";
    location.reload();
}

saveComment.onclick= async function(event){
    const commentbody = {
        patientURN: selectedBed.patientURN,
        staffID: parseInt(staffMemberId.value),
        loggedComment: commentText.value
    }
    
    await addComment(commentbody);
    comment.style.display = "none";
    location.reload();
}

//close buttons for dialogue windows
admitSpan.onclick = function(event) {
    admit.style.display = "none";
}

patientSpan.onclick = function(event) {
    newPatient.style.display = "none";
}
dischargeSpan.onclick = function(event) {
    discharge.style.display = "none";
}
cancelDischarge.onclick = function(event) {
    discharge.style.display = "none";
}
commentSpan.onclick = function(event) {
    comment.style.display = "none";
}

patientDetailsSpan.onclick = function(event) {
    patientDetails.style.display = "none";
}

//close diaglogue windows when clicking outside of it
window.onclick = function(event) {
    if (event.target == admit) {
      admit.style.display = "none";
    }
    if (event.target == discharge) {
        discharge.style.display = "none";
    }
    if (event.target == comment) {
        comment.style.display = "none";
    }
    if (event.target == newPatient) {
        newPatient.style.display = "none";
    }
    if (event.target == patientDetails) {
        patientDetails.style.display = "none";
    }
      
}