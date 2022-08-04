const getOrCreateTooltip = (chart) => {
    let tooltipEl = chart.canvas.parentNode.querySelector('div');

    if (!tooltipEl) {
        tooltipEl = document.createElement('div');
        tooltipEl.style.background = 'rgba(0, 0, 0, 0.7)';
        tooltipEl.style.borderRadius = '3px';
        tooltipEl.style.color = 'white';
        tooltipEl.style.opacity = 1;
        tooltipEl.style.pointerEvents = 'none';
        tooltipEl.style.position = 'absolute';
        tooltipEl.style.transform = 'translate(-50%, 0)';
        tooltipEl.style.transition = 'all .1s ease';
        tooltipEl.style.textAlign = "center";
        const table = document.createElement('table');
        table.style.margin = '0px';

        tooltipEl.appendChild(table);
        chart.canvas.parentNode.appendChild(tooltipEl);
    }

    return tooltipEl;
};

const externalTooltipHandler = (context) => {
    //console.log(context);
    // Tooltip Element
    const { chart, tooltip } = context;
    const tooltipEl = getOrCreateTooltip(chart);

    // Hide if no tooltip
    if (tooltip.opacity === 0) {
        tooltipEl.style.opacity = 0;
        return;
    }

    // Set Text
    if (tooltip.body) {

        //const titleLines = tooltip.title || [];
        //const bodyLines = tooltip.body.map(b => b.lines);

        const tableHead = document.createElement('thead');

        const tr = document.createElement('tr');
        tr.style.borderWidth = 0;
        let lenOfStudy = tooltip.dataPoints[0].formattedValue;
        let typeOfStudy = tooltip.dataPoints[0].dataset.types[tooltip.dataPoints[0].dataIndex];

        let title = lenOfStudy + " " + typeOfStudy;
        const th = document.createElement('th');
        th.style.borderWidth = 0;
        const text = document.createTextNode(title);
        th.appendChild(text);
        tr.appendChild(th);
        tableHead.appendChild(tr);


        const tableBody = document.createElement('tbody');
        //Date of Study
        const tr1 = document.createElement('tr');
        tr1.style.backgroundColor = 'inherit';
        tr1.style.borderWidth = 0;

        const td1 = document.createElement('td');
        td1.style.borderWidth = 0;

        //let dateOfStudy = tooltip.title[0];
        let dateOfStudy = tooltip.dataPoints[0].label;
        const dateStudyText = document.createTextNode(dateOfStudy);

        td1.appendChild(dateStudyText);
        tr1.appendChild(td1);
        tableBody.appendChild(tr1);
        //chapter of study

        const tr2 = document.createElement('tr');
        tr2.style.backgroundColor = 'inherit';
        tr2.style.borderWidth = 0;

        const td2 = document.createElement('td');
        td2.style.borderWidth = 0;

        //const text = document.createTextNode(body);
        let chapterOfStudy = tooltip.dataPoints[0].dataset.chapters[tooltip.dataPoints[0].dataIndex];
        const chapterStudyText = document.createTextNode(chapterOfStudy);

        td2.appendChild(chapterStudyText);
        tr2.appendChild(td2);
        tableBody.appendChild(tr2);


        const tableRoot = tooltipEl.querySelector('table');

        // Remove old children
        while (tableRoot.firstChild) {
            tableRoot.firstChild.remove();
        }

        // Add new children
        tableRoot.appendChild(tableHead);
        tableRoot.appendChild(tableBody);
    }

    const { offsetLeft: positionX, offsetTop: positionY } = chart.canvas;

    // Display, position, and set styles for font
    tooltipEl.style.opacity = 1;
    tooltipEl.style.left = positionX + tooltip.caretX + 'px';
    tooltipEl.style.top = positionY + tooltip.caretY + 'px';
    tooltipEl.style.font = tooltip.options.bodyFont.string;
    tooltipEl.style.padding = tooltip.options.padding + 'px ' + tooltip.options.padding + 'px';
};





function makeChart(numbers, chapters, types, labels) {
    try {
        const ctx = document.getElementById('myChart').getContext('2d');
        const data = {
            //labels: ['1401/01/29', '1401/01/20', '1401/01/18', '1401/01/15', '1401/01/12', '1401/01/10'],
            labels: labels,
            datasets: [
                {
                    label: 'مطالعات',
                    fill: false,
                    //borderColor: 'rgb(235, 52, 52)',
                    //backgroundColor: 'rgba(235, 52, 52,0.5)',
                    //data: [15, 21, 17, 9, 8],
                    //chapters: ["فصل1", "فصل1", "فصل2", "فصل2", "فصل2"],
                    //types: ["صفحه", "صفحه", "صفحه", "دقیقه", "دقیقه"],

                    borderColor: '#B388FF',
                    backgroundColor: '#311B92',
                    data: numbers,
                    chapters: chapters,
                    types: types,
                    tension: 0.5
                }
            ]
        };
        const myChart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                plugins: {
                    //title: {
                    //    display: true,
                    //    text: 'Chart.js Line Chart - External Tooltips'
                    //},
                    tooltip: {
                        enabled: false,
                        position: 'nearest',
                        external: externalTooltipHandler,
                    },
                    legend: {
                        labels: {
                            // This more specific font property overrides the global property
                            font: {
                                size: 18,
                                family: 'IRANSans'
                            }
                        }
                    }
                }
            }
        });

    } catch (e) {
        console.log(e)
    }
}
