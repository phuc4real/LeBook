//API
const getCate = "/Customer/Book/GetData?type=cate";
const getAge = "/Customer/Book/GetData?type=age";
const getCovT = "/Customer/Book/GetData?type=covertype";

// Element
const cateholder = document.querySelector('#category');
const ageholder = document.querySelector('#age');
const covtholder = document.querySelector('#covertype');

async function addData(holder,api,type) {
    const response = await fetch(api);
    const data = await response.json();
    holder.innerHTML = '';
    data.forEach(item => {
        const li = document.createElement('li');
        li.classList.add('py-1');
        li.innerHTML = '<a href="/Customer/Book/Find/' + item.id + '?findby=' + type + '">' + item.name + '</a>';
        holder.appendChild(li);
    });
}

addData(cateholder, getCate, 'cate');
addData(ageholder, getAge, 'age');
addData(covtholder, getCovT, 'covertype');