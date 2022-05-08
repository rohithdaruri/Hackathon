import React, { Component } from 'react';

export class Records extends Component {
    static displayName = Records.name;

    constructor(props) {
        super(props);
        this.state = { records: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderRecordsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>UniqueId</th>
                        <th>InputText</th>
                        <th>Language</th>
                        <th>Type</th>
                        <th>BlobPath</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.id}>
                            <td>{forecast.uniqueId}</td>
                            <td>{forecast.inputText}</td>
                            <td>{forecast.language}</td>
                            <td>{forecast.processType}</td>
                            <td>{forecast.blobPath}</td>  
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Records.renderRecordsTable(this.state.records);

        return (
            <div>
                <h1 id="tabelLabel" >Records</h1>
                {contents}
            </div>
        );
    }

    async populateData() {
        const response = await fetch('/api/TextToSpeech/Records');
        const data = await response.json();
        this.setState({ records: data, loading: false });
    }
}
