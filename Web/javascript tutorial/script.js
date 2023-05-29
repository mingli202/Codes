const d = document;

async function init() {
    const response = await fetch('./data.json');
    window.data = await response.json();

    display(data);
}

function display(dataObj) {
    const grid = d.querySelector('#grid'); // get the grid element

    for (let i = 0; i < dataObj.length; i++) {

        const gridItem = d.createElement('div'); // create a new div element for the sushi
        gridItem.className = 'grid-item'; // add a the grid-item class to the new div element

        const sushiName = d.createElement('p'); // create a new p element for the sushi name
        sushiName.className = 'sushi-name'; // add a the sushi-name class to the new p
        sushiName.innerHTML = dataObj[i].name; // add the sushi name to the new p
        gridItem.appendChild(sushiName); // add the sushi name to the new div element
        
        const sushiContainer = document.createElement('div');
        sushiContainer.className ='sushi-container';

        const imageContainer = d.createElement('div');
        imageContainer.className = 'image-container';
        const sushiImage = d.createElement('img');
        sushiImage.className ='sushi-image';
        sushiImage.src = dataObj[i].img;
        imageContainer.appendChild(sushiImage);
        sushiContainer.appendChild(imageContainer);

        const sushiInfo = d.createElement('div');
        sushiInfo.className ='sushi-info';
        const sushiPrice = d.createElement('p');
        sushiPrice.innerHTML = '$' + dataObj[i].price;
        sushiInfo.appendChild(sushiPrice);

        for (let j = 0; j < dataObj[i].desc.length; j++) {
            const sushiDesc = d.createElement('p');
            sushiDesc.innerHTML = dataObj[i].desc[j];
            sushiInfo.appendChild(sushiDesc);
        }

        sushiContainer.appendChild(sushiInfo);
        gridItem.appendChild(sushiContainer);
        grid.appendChild(gridItem);
    }
}

function home() {
    window.location.href = './index.html';
}

// animate .explore when div shows in viewport
const element = document.querySelector('.fresh-image');
const observer = new IntersectionObserver(entries => {
    element.classList.toggle( 'animate', entries[0].isIntersecting );
  });
  
observer.observe( element );
