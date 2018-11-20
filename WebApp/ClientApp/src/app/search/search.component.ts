import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html'
})
export class SearchComponent {
  public searchResults: SearchResult[];

  constructor(http: HttpClient, @Inject('API_URL') baseUrl: string) {
    http.get<SearchResult[]>(baseUrl + 'search').subscribe(result => {
      this.searchResults = result;
    }, error => console.error(error));
  }
}

interface SearchResult {

}
