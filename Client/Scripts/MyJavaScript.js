function signin() {
    document.getElementById('resigter').style.display = 'none';
    document.getElementById('signin').style.display ='block';
     
}
function resigter() {
    document.getElementById('signin').style.display = 'none';
    document.getElementById('resigter').style.display = 'block' ;
   }

function getLogin() {
    var account = {
        userName: $('#userName').val(),
        pass_word: $('#pass_word').val()
    };
    $.ajax({
        url: 'http://localhost:61143/api/account/Login/',
        method: 'POST',
        dataType: 'json',
        contextType: 'application/json',
        data: JSON.stringify(account),
        success: function (response) {
            alert("Hello! I am an alert box!!");
            sessionStorage.setItem('accessToken', response.access_token);
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}
function login() {
    var response = client.GetStringAsync(url);
    var rootObject = JsonConvert.DeserializeObject<RootObject>(response.Result);
}

// Get the Sidebar
var mySidebar = document.getElementById("mySidebar");

// Get the DIV with overlay effect
var overlayBg = document.getElementById("myOverlay");

// Toggle between showing and hiding the sidebar, and add overlay effect
function w3_open() {
    if (mySidebar.style.display === 'block') {
        mySidebar.style.display = 'none';
        overlayBg.style.display = "none";
    } else {
        mySidebar.style.display = 'block';
        overlayBg.style.display = "block";
    }
}

// Close the sidebar with the close button
function w3_close() {
    mySidebar.style.display = "none";
    overlayBg.style.display = "none";
}

function changeImage(input) {
    var formdata = new FormData();
    formdata.append("image", input.files[0]);
    $.ajax({
        contentType: false,
        processData: false,
        url: 'https://api.imgur.com/3/image',
        type: 'POST',
        headers: {
            Authorization: 'Client-ID f2e293cfd80c0d7',
        }, mimeType: 'multipart/form-data',
        data: formdata,
         beforeSend: () => {
             $('#loader').show();
        }
        ,
        complete: () => {
            $('#loader').hide();
        },
        error: function (result) {
            console.log(result);
        }
        ,
        success: function (result) {
            result = JSON.parse(result); 
            var id = result.data.id;
            $('#urlImg').val('https://imgur.com/' + id + '.jpg')  ;
            $('#Image').attr('src', 'https://imgur.com/' + id +'.jpg');
        }
       
    });

}
function CheckInfo(element, type) {
    var regexp;
    var message;
    switch (type) {
        case "phone":
            regexp = /^[0-9]$/;
            message = 'Please Enter Number Only';
            break;
        case "Confpassword":
            regexp = $('#Confpassword').val;
            if (regexp == element.val()) {
                message = 'Please Enter Number Only';
            }
            break;
        default:
    }
    if (message != null) {
        element.placeholder = message;
        element.classList.add('border border-dangerous');
    } else {
        element.classList.add('border border-success');
    }
}
function changeDateF() {
    var dateStt = new Date(document.getElementById("inpDateStt").value);
    var dateEnd = new Date(document.getElementById("inpDateEnd").value);
    var e = document.getElementById("idCat");
    var opt = e.options[e.selectedIndex].value;
    var Difference_In_Time = dateEnd.getTime() - dateStt.getTime();
    var Difference_In_Days = (Difference_In_Time / (1000 * 3600 * 24)) + 1;
    if (!isNaN(Difference_In_Days)) {
        document.getElementById("Difference_In_Days").innerHTML = Difference_In_Days;
        document.getElementById("subTotal").innerHTML = Difference_In_Days * opt;
    }
}
function addToCarts(id) {
    var dateStt = document.getElementById("inpDateStt").value;
    var dateEnd = document.getElementById("inpDateEnd").value;
    if (dateStt > dateEnd) {
        alert("Date end must be equal or greater than date start");
    } else {
        location.href = '/Customer/AddToCart?staffId=' + id + '&dateSTT=' + dateStt +
            '&dateEND=' + dateEnd +
            '&amountMoney=' + document.getElementById("subTotal").textContent;
    }
}