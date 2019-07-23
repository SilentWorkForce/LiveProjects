/********DASHBOARD SCRIPTS**********/

console.log('testing hover');
   
    /* Popover effects for Dashboard app icons */

    var bindToMe_1 = document.createElement('a')    
    $('#target1').popover({
        content: bindToMe_1,                
        placement: 'left',
        trigger: 'hover',
        delay: {
            show: 1000,                /* 500 = half a second, 1000 = 1 second, etc... In case you want to adjust the delay for longer or shorter */
        },
        html: true
    });    
    bindToMe_1.innerHTML = "<em>Description:</em> <br> Take the opportunity to plan steps of your upcoming trips.  Check them off as you go.  <br> CLICK THE APP ICON TO GET STARTED!!";

    var bindToMe_2 = document.createElement('a')  
    $('#target2').popover({
        content: bindToMe_2,    
        placement: 'top',
        trigger: 'hover',
        delay: {
            show: 1000,
        },
        html: true
    });    
    bindToMe_2.innerHTML = "<em>Description:</em> <br> Take the opportunity to plan expenses of your upcoming trips.  Make adjustments to give you the best experience possible! <br> CLICK THE APP ICON TO GET STARTED!!";
    
    var bindToMe_3 = document.createElement('a')   
    $('#target3').popover({
        content: bindToMe_3,        
        placement: 'top',
        trigger: 'hover',
        delay: {
            show: 1000,
        },
        html: true
    });     
    bindToMe_3.innerHTML = "<em>Description:</em> <br> Take the opportunity to research potential travel destinations.  Look up current reports to ensure a safe trip. <br> CLICK THE APP ICON TO GET STARTED!!";

    var bindToMe_4 = document.createElement('a')    
    $('#target4').popover({
        content: bindToMe_4,       
        placement: 'top',
        trigger: 'hover',
        delay: {
            show: 1000,
        },
        html: true
    });      
    bindToMe_4.innerHTML = "<em>Description:</em> <br> Take the opportunity to examine the current currency exchange between home and abroad.  <br> CLICK THE APP ICON TO GET STARTED!!";
        
   
    var bindToMe_5 = document.createElement('a')    
    $('#target5').popover({
        content: bindToMe_5,
        placement: "left",
        trigger: 'hover',
        delay: {
            show: 1500,
        },
        html: true
    });    
    bindToMe_5.innerHTML = "<em>Description:</em> <br> Take the opportunity to find information of flights available for your next trip.  <br> CLICK THE APP ICON TO GET STARTED!!";
    
    var bindToMe_6 = document.createElement('a')
    $('#target6').popover({
        content: bindToMe_6,
        placement: "bottom",
        trigger: 'hover',
        delay: {
            show: 1500,
        },
        html: true
    });    
    bindToMe_6.innerHTML = "<em>Description:</em> <br> Take the opportunity to research hotel and lodging options for your next trip.  <br> CLICK THE APP ICON TO GET STARTED!!";

    var bindToMe_7 = document.createElement('a')
    $('#target7').popover({
        content: bindToMe_7,
        placement: 'bottom',
        trigger: 'hover',
        delay: {
            show: 1500,
        },
        html: true
    });    
    bindToMe_7.innerHTML = "<em>Description:</em> <br> Chat about your findings with your friends and colleagues.  <br> CLICK THE APP ICON TO GET STARTED!!";

    var bindToMe_8 = document.createElement('a')
    $('#target8').popover({
        content: bindToMe_8,
        placement: 'bottom',
        trigger: 'hover',
        delay: {
            show: 1500,
        },
        html: true
    });    
    bindToMe_8.innerHTML = "This is what the Future App is for...Currently under construction";



















/*  JS for the Budget Application  */

(function(){

    document.querySelector('#categoryInput').addEventListener('keydown', function(e){
        if (e.keyCode != 13){
            return;
        }

        e.preventDefault()

        var categoryName = this.value
        this.value = ''
        addNewCategory(categoryName)
        updateCategoriesString()
    })

    function addNewCategory(name){

        document.querySelector('#categoriesContainer').insertAdjacentHTML('beforeend',
        `<li class="category">
            <span class="name">${name}</span>
            <span onclick="removeCategory(this)" class="btnRemove bold">X</span>
        </li>`)
    }

    

})()

function fetchCategoryArray(){
    var categories = []

    document.querySelectorAll('.category').forEach(function(e){
        name = e.querySelector('.name').innerHTML
        if (name == '') return

        categories.push(name)
    })

    return categories
}

function updateCategoriesString(){
    categories = fetchCategoryArray()
    document.querySelector('input[name="categoriesString"]').value = categories.join(',')

} 

function removeCategory(e){
    e.parentElement.remove()
    updateCategoriesString()
}


