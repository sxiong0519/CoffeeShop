const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
const beansContainer = document.querySelector("#beanVarieties")


button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            beansContainer.innerHTML = ""

            const beanList = document.createElement("ul")
            console.log(beanList)
            for( const variety of beanVarieties) {
                beanList.innerHTML += `<li><p>${variety.name}</p></li>`
            }
            beansContainer.appendChild(beanList)
            displayBeanForm()
        })
});

function getAllBeanVarieties() {
    return fetch(url).then(resp => resp.json());
}

function displayBeanForm() {
    const beanFormWrapper = document.querySelector("#beanForm-wrapper");
    beanFormWrapper.innerHTML = "";
    const beanForm = document.createElement("article")
    beanForm.classList.add("beanForm")
    beanForm.innerHTML = `
        <h3>Add new bean variety</h3>
        <input id="coffee-name"type="text" placeholder="variety name">
        <input id="coffee-region"type="text" placeholder="region">
        <textarea id="coffee-notes" placeholder="notes (optional)" rows="5" cols="40"></textarea>
        <button id="coffee-btn">Save</button>
    `
    beanFormWrapper.appendChild(beanForm)
    document.querySelector("#coffee-btn").addEventListener('click', saveNewBean)
}

function saveNewBean() {
    const newBean = {
        name: document.querySelector("#coffee-name").value,
        region: document.querySelector("#coffee-region").value,
        notes: document.querySelector("#coffee-notes").value
    }
    fetch('https://localhost:5001/api/BeanVariety', {
        method: 'POST',
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify(newBean)       
    })
    .then(resp => resp.json())
    .then( newBean => alert(`New bean variety, ${newBean.name}, created!`))
}