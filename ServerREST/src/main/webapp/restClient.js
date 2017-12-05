function doGet(){
	$.ajax({
    	type: "GET",
    	 contentType: "application/json; charset=utf-8",
         dataType: "json",
        url: "http://localhost:8080/restws/services/patientservice/patients/"+$('#patientID').val()
    }).then(function(data) {
       $('#patientID').val(data.id);
       $('#patientName').val(data.name);
    });
}
	
function doPost(){
$.ajax({
	type: "POST",
	 contentType: "application/json; charset=utf-8",
     dataType: "json",
     data: JSON.stringify({name:$('#patientName').val()}),
    url: "http://localhost:8080/restws/services/patientservice/patients"
    	
	}).then(function(data) {
		$('#patientID').val(data.id);
		$('#patientName').val(data.name);
	});
    
}
	
	
function doDelete(){
	$.ajax({
    	type: "DELETE",
    	 contentType: "application/json; charset=utf-8",
         dataType: "json",
        url: "http://localhost:8080/restws/services/patientservice/patients/"+$('#patientID').val()
    }).then(function(data) {
       alert('Patient Deleted');
    });
}